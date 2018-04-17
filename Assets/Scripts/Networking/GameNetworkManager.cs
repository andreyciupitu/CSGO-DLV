using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

namespace CSGO_DLV.Networking
{
    [RequireComponent(typeof(LobbyHook))]
    public class GameNetworkManager : NetworkLobbyManager
    {
        private static GameNetworkManager instance = null;
        private bool inMatchMaking;

        /// <summary>
        /// LobbyHook must be on the same GameObject as the GameNetworkManager
        /// </summary>
        private LobbyHook lobbyHook;

        // Server info
        public string ServerStatus { get; set; }

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

        private void Awake()
        {
            // Singleton Pattern
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Destroy(gameObject); // Keep only one instance
                return;
            }

            DontDestroyOnLoad(gameObject);
            lobbyHook = GetComponent<LobbyHook>();
            inMatchMaking = false;
        }
        
        /// <summary>
        /// Get the next spawn position, taking into account teams and oher criteria.
        /// </summary>
        /// <returns> The next spawn transform. </returns>
        public new Transform GetStartPosition()
        {
            // TODO: Add implementation based on Lobby Players here.

            return base.GetStartPosition();
        }

        ////////////////////////// Game Handling //////////////////////////// 

        /// <summary>
        /// Hosts a new LAN game 
        /// </summary>
        public void HostLanGame()
        {
            StartHost();
            ServerStatus = "LAN Host";
        }

        /// <summary>
        /// Joins a LAN game lobby at a given address.
        /// </summary>
        /// <param name="gameAddress"> Network address of the game </param>
        public void JoinLanGame(string gameAddress)
        {
            networkAddress = gameAddress;
            StartClient();
            ServerStatus = "Connecting";
        }

        /// <summary>
        /// Leaves a LAN lobby, and closes the server if the player was a host
        /// </summary>
        public void LeaveLanGame()
        {
            if (NetworkServer.active)
            {
                StopHost();
            }
            else
            {
                StopClient();
            }
            ServerStatus = "Offline";
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
            ServerStatus = "MM server " + this.matchName;
        }

        /// <summary>
        /// Joins a MM match with the given ID
        /// </summary>
        /// <param name="networkID"></param>
        /// <param name="password"> Optional, no password default </param>
        public void JoinMatch(NetworkID networkID, string password = "")
        {
            matchMaker.JoinMatch(networkID, password, "", "", 0, 0, OnMatchJoined);
            inMatchMaking = true;
        }

        /*
        public void GetMatchPage(int page)
        {
            matchMaker.ListMatches(page, 6, "", true, 0, 0, OnGUIMatchList);
        }*/

        public void LeaveMatch()
        {
            if (NetworkServer.active)
            {
                matchMaker.DestroyMatch(matchInfo.networkId, 0, OnDestroyMatch);
                StopHost();
            }
            else
            {
                StopClient();
            }
            ServerStatus = "Offline";
        }

        ////////////////////////// Callbacks //////////////////////////// 

        public override void OnLobbyStartHost()
        {
            lobbyHook.OnLobbyStartHost.Invoke();
        }

        public override void OnLobbyClientConnect(NetworkConnection conn)
        {
            ServerStatus = "LAN Client";
            lobbyHook.OnLobbyClientConnect.Invoke(conn);
        }

        public override void OnLobbyClientDisconnect(NetworkConnection conn)
        {
            ServerStatus = "Offline";
            lobbyHook.OnLobbyClientDisconnect.Invoke(conn);
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            lobbyHook.OnLobbyClientSceneChanged.Invoke(conn);
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            Debug.Log("Game started!");
            return lobbyHook.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            Debug.Log("Adding a new player!");
            Instantiate(playerPrefab, GetStartPosition());
        }

        public override void OnLobbyServerPlayersReady()
        {
            base.OnLobbyServerPlayersReady();
        }

        public override void OnLobbyStopHost()
        {
            base.OnLobbyStopHost();
        }

        public override void OnLobbyStartServer()
        {
            base.OnLobbyStartServer();
        }

        public override void OnLobbyServerConnect(NetworkConnection conn)
        {
            base.OnLobbyServerConnect(conn);
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            base.OnLobbyServerDisconnect(conn);
        }

        public override void OnLobbyServerSceneChanged(string sceneName)
        {
            base.OnLobbyServerSceneChanged(sceneName);
        }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        }

        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
            base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
        }

        public override void OnLobbyClientEnter()
        {
            base.OnLobbyClientEnter();
        }

        public override void OnLobbyClientExit()
        {
            base.OnLobbyClientExit();
        }

        public override void OnLobbyStartClient(NetworkClient lobbyClient)
        {
            base.OnLobbyStartClient(lobbyClient);
        }

        public override void OnLobbyStopClient()
        {
            base.OnLobbyStopClient();
        }

        public override void OnLobbyClientAddPlayerFailed()
        {
            base.OnLobbyClientAddPlayerFailed();
        }
    }

}
