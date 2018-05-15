using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

namespace CSGO_DLV.Networking
{
    public class KickMsg : MessageBase { }

    [System.Serializable]
    public class ServerStatus
    {
        public string Status { get; set; }
        public bool IsServerOnly { get; set; }
        public bool IsMatchMaking { get; set; }
        public bool IsHost { get; set; }
    }

    //[RequireComponent(typeof(LobbyHook))]
    public class GameNetworkManager : NetworkLobbyManager
    {
        private static GameNetworkManager instance = null;
        private const short MsgKicked = MsgType.Highest + 1;

        private ServerStatus serverStatus;
        private LobbyHook lobbyHook;
        private int playerCount = 0;
        private string originalLobby;
        [SerializeField]
        private float prematchCountdown;

        /// <summary>
        /// Singleton instance of the GameNetworkManager
        /// </summary>
        public static GameNetworkManager Manager
        {
            get
            {
                return instance;
            }
        }
        public int PlayerCount
        {
            get
            {
                return playerCount;
            }
        }

        private void Awake()
        {
            // Sort of Singleton Pattern - keep the new instance
            // This is so the events & references set in the editor
            // don't get lost when the other instance is destroyed
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Destroy(instance.gameObject); // Keep only one instance
                instance = this;
            }

            DontDestroyOnLoad(gameObject);
            lobbyHook = GetComponent<LobbyHook>();
            serverStatus = new ServerStatus();
            originalLobby = offlineScene;
            offlineScene = "";
        }
        

        #region GameHandling API
        /// <summary>
        /// Hosts a new LAN game 
        /// </summary>
        public void HostLanGame()
        {
            StartHost();
            serverStatus.Status = "LAN Host";
            serverStatus.IsServerOnly = false;
            serverStatus.IsHost = true;
            serverStatus.IsMatchMaking = false;

            lobbyHook.OnInfoUpdate(serverStatus.Status);
        }

        /// <summary>
        /// Start a new LAN server
        /// </summary>
        public void DedicatedLANServer()
        {
            StartServer();
            serverStatus.Status = "LAN Server";
            serverStatus.IsServerOnly = true;
            serverStatus.IsHost = true;
            serverStatus.IsMatchMaking = false;

            lobbyHook.OnInfoUpdate(serverStatus.Status);
        }

        /// <summary>
        /// Joins a LAN game lobby at a given address.
        /// </summary>
        /// <param name="gameAddress"> Network address of the game </param>
        public void JoinLanGame(string gameAddress)
        {
            networkAddress = gameAddress;
            StartClient();
            serverStatus.Status = "Connecting";
            serverStatus.IsMatchMaking = true;
            serverStatus.IsServerOnly = false;
            serverStatus.IsHost = false;

            lobbyHook.OnInfoUpdate(serverStatus.Status);
        }

        /// <summary>
        /// Creates a new MM match
        /// </summary>
        /// <param name="matchName"></param>
        /// <param name="password"> Optional, no password default</param>
        public void CreateMatch(string matchName, string password = "")
        {
            StartMatchMaker();
            matchMaker.CreateMatch(matchName, (uint)maxPlayers, true, password, "", "", 0, 0, OnMatchCreate);
            serverStatus.Status = "MM server " + this.matchName;
            serverStatus.IsMatchMaking = true;
            serverStatus.IsServerOnly = false;
            serverStatus.IsHost = true;

            lobbyHook.OnInfoUpdate(serverStatus.Status);
        }

        /// <summary>
        /// Joins a MM match with the given ID
        /// </summary>
        /// <param name="networkID"></param>
        /// <param name="password"> Optional, no password default </param>
        public void JoinMatch(NetworkID networkID, string password = "")
        {
            matchMaker.JoinMatch(networkID, password, "", "", 0, 0, OnMatchJoined);
            serverStatus.IsMatchMaking = true;
            serverStatus.IsServerOnly = false;
            serverStatus.IsHost = false;
        }

        /*
        public void GetMatchPage(int page)
        {
            matchMaker.ListMatches(page, 6, "", true, 0, 0, OnGUIMatchList);
        }*/
        
        /// <summary>
        /// Leaves a lobby, and closes the server if the player was a host
        /// </summary>
        public void LeaveGame()
        {
            if (serverStatus.IsHost)
            {
                if (serverStatus.IsMatchMaking)
                    matchMaker.DestroyMatch(matchInfo.networkId, 0, OnDestroyMatch);

                if (serverStatus.IsServerOnly)
                    StopServer();
                else
                    StopHost();
            }
            else
            {
                if (client != null && client.connection != null)
                    OnLobbyClientDisconnect(this.client.connection);
                StopClient();
            }

            serverStatus.Status = "Offline";
            serverStatus.IsMatchMaking = false;
            serverStatus.IsServerOnly = false;
            serverStatus.IsHost = false;
            
            lobbyHook.OnInfoUpdate(serverStatus.Status);
        }

        /// <summary>
        /// Kicks the player on the given connection
        /// </summary>
        /// <param name="conn">Connection with the player</param>
        public void KickPlayer(GameLobbyPlayer player)
        {
            player.connectionToClient.Send(MsgKicked, new KickMsg());
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Begins a countdown before starting the match.
        /// </summary>
        private IEnumerator StartMatch()
        {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                // Update the info panel on each client
                if (newFloorTime != floorTime)
                {
                    floorTime = newFloorTime;

                    serverStatus.Status = "Starting Game in " + newFloorTime + " seconds!";
                    UpdateDisplayedInfo();
                }
            }

            serverStatus.Status = "Loading map...";
            UpdateDisplayedInfo();
            
            // Start game when timer reaches 0
            GameNetworkManager.Manager.ServerChangeScene(playScene);
        }
        
        /// <summary>
        /// Handler for Kick messages
        /// </summary>
        private void KickedMessageHandler(NetworkMessage netMsg)
        {
            netMsg.conn.Disconnect();
            lobbyHook.OnPlayerKicked();
        }

        /// <summary>
        /// Updates the server status on all clients
        /// </summary>
        private void UpdateDisplayedInfo()
        {
            foreach (NetworkLobbyPlayer p in lobbySlots)
            {
                if (p != null)
                {
                    (p as GameLobbyPlayer).RpcUpdateInfo(serverStatus.Status);
                }
            }
        }
        #endregion

        #region Callbacks

        //TODO Add On stop Client override
        // Find out how to notify added players
        public override void OnLobbyStartHost()
        {
            lobbyHook.OnStartHost();
        }

        public override void OnLobbyStopHost()
        {
            lobbyHook.OnStopHost();
        }

        public override void OnLobbyStartServer()
        {
            lobbyHook.OnStartServer();
        }

        public override void OnStopServer()
        {
            lobbyHook.OnStopServer();
        }
        
        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        }

        public override void OnLobbyServerPlayersReady()
        {
            StartCoroutine(StartMatch());
        }

        public override void OnLobbyClientConnect(NetworkConnection conn)
        {
            // Add Kick Handler
            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            // Update server info when connection is established
            if (!serverStatus.IsHost)
            {
                serverStatus.Status = "LAN Client";
                lobbyHook.OnClientConnect(conn);
                lobbyHook.OnInfoUpdate(serverStatus.Status);
            }
        }

        public override void OnLobbyClientDisconnect(NetworkConnection conn)
        {
            lobbyHook.OnClientDisconnect(conn);
        }

        public override void OnLobbyClientAddPlayerFailed()
        {
            lobbyHook.OnClientAddError();
        }
        #endregion
    }
}
