using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CSGO_DLV.Menu;

namespace CSGO_DLV.Networking
{
    public class GameLobbyPlayer : NetworkLobbyPlayer
    {
        [System.Serializable]
        public struct Skin
        {
            public string name;
            public Color skin;
        }

        private PlayerEntry entry;
        private LobbyHook lobbyHook;

        [SerializeField]
        private Skin[] skins; // editor display
        
        [SyncVar(hook = "OnNameChanged")]
        private string playerName = "cacat";
        [SyncVar(hook = "OnSkinChanged")]
        private int skin = 0;

        public string Name
        {
            get
            {
                return playerName;
            }
        }
        public int SkinNumber
        {
            get
            {
                return skin;
            }
        }
        public Skin[] Skins
        {
            get
            {
                return skins;
            }
        }
        public PlayerEntry Entry
        {
            get
            {
                return entry;
            }
            set
            {
                entry = value;
                entry.SetToPlayer(this);
            }
        }

        private void Awake()
        {
            lobbyHook = LobbyHook.Hook;
        }

        #region PlayerSetup
        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();
            lobbyHook.OnPlayerAdded();
        }

        private void OnDestroy()
        {
            GameNetworkManager.Manager.lobbySlots[slot] = null; // Hardcoded to make sure the refresh works
            lobbyHook.OnPlayerRemoved();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            // Default player name
            CmdUpdateName("Player #" + (slot + 1));

            // Local player listens to the ready button
            lobbyHook.RegisterPlayerReadyHandler(SendReadyToBeginMessage);

            // Update the asociated entry for the local player
            entry.LocalPlayerSetup();
            entry.UpdateInfo();
        }
 
        public override void OnClientReady(bool readyState)
        {
            base.OnClientReady(readyState);
            entry.UpdateInfo();
        }

        public void ChangeName(string name)
        {
            CmdUpdateName(name);
        }

        public void ChangeSkin(int value)
        {
            CmdChangeSkin(value);
        }
        #endregion

        #region SyncVars
        private void OnNameChanged(string name)
        {
            playerName = name;
            entry.UpdateInfo();
        }

        private void OnSkinChanged(int skin)
        {
            this.skin = skin;
            entry.UpdateInfo();
        }
        #endregion
        
        #region Commands
        [Command]
        private void CmdUpdateName(string name)
        {
            playerName = name;
        }

        [Command]
        private void CmdChangeSkin(int skin)
        {
            this.skin = skin;
        }
        #endregion

        #region RPCs
        /// <summary>
        /// Fires an info updated event on every client.
        /// </summary>
        /// <param name="info">New server info</param>
        [ClientRpc]
        public void RpcUpdateInfo(string info)
        {
            lobbyHook.OnInfoUpdate(info);
        }
        #endregion
    }
}
