using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace CSGO_DLV.Networking
{
    // Custom events
    [Serializable]
    public class ConnectionEvent : UnityEvent<NetworkConnection> { }
    [Serializable]
    public class TextUpdateEvent : UnityEvent<string> { }
    [Serializable]
    public class MatchListEvent : UnityEvent<List<MatchInfoSnapshot>> { }

    public class LobbyHook : MonoBehaviour
    {
        private static LobbyHook instance = null;

        #region Events
        [Header("Host events")]
        [SerializeField]
        private UnityEvent onStartHost = new UnityEvent();
        [SerializeField]
        private UnityEvent onStopHost = new UnityEvent();

        [Header("Matchmaking events")]
        [SerializeField]
        private UnityEvent onMatchCreate = new UnityEvent();
        [SerializeField]
        private UnityEvent onMatchJoined = new UnityEvent();
        [SerializeField]
        private MatchListEvent onMatchList = new MatchListEvent();
 
        [Header("Player events")]
        [SerializeField]
        private UnityEvent onPlayerKicked = new UnityEvent();
        [SerializeField]
        private UnityEvent onPlayerReady = new UnityEvent();
        [SerializeField]
        private TextUpdateEvent onInfoUpdate = new TextUpdateEvent();
        [SerializeField]
        private UnityEvent onAddPlayer = new UnityEvent();
        [SerializeField]
        private UnityEvent onRemovePlayer = new UnityEvent();

        [Header("Clientside callbacks")]
        [SerializeField]
        private ConnectionEvent onClientConnect = new ConnectionEvent();
        [SerializeField]
        private ConnectionEvent onClientDisconnect = new ConnectionEvent();
        [SerializeField]
        private UnityEvent onClientAddError = new UnityEvent();

        [Header("Serverside callbacks")]
        [SerializeField]
        private UnityEvent onStartServer = new UnityEvent();
        [SerializeField]
        private UnityEvent onStopServer = new UnityEvent();
        #endregion

        /// <summary>
        ///The active instance of the LobbyHook
        /// </summary>
        public static LobbyHook Hook
        {
            get
            {
                return instance;
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
        }

        #region Handlers API
        public void PlayerReady()
        {
            onPlayerReady.Invoke();
        }

        public void RegisterPlayerReadyHandler(UnityAction handler)
        {
            onPlayerReady.AddListener(handler);
        }

        public void OnPlayerKicked()
        {
            onPlayerKicked.Invoke();
        }

        public void RegisterPlayerKickedHandler(UnityAction handler)
        {
            onPlayerKicked.AddListener(handler);
        }

        public void OnInfoUpdate(string s)
        {
            onInfoUpdate.Invoke(s);
        }

        public void RegisterInfoUpdatedHandler(UnityAction<string> handler)
        {
            onInfoUpdate.AddListener(handler);
        }

        public void OnStartHost()
        {
            onStartHost.Invoke();
        }

        public void RegisterStartHostHandler(UnityAction handler)
        {
            onStartHost.AddListener(handler);
        }

        public void OnStopHost()
        {
            onStopHost.Invoke();
        }

        public void RegisterStopHostHandler(UnityAction handler)
        {
            onStopHost.AddListener(handler);
        }

        public void OnClientConnect(NetworkConnection conn)
        {
            onClientConnect.Invoke(conn);
        }

        public void RegisterClientConnectHandler(UnityAction<NetworkConnection> handler)
        {
            onClientConnect.AddListener(handler);
        }

        public void OnClientDisconnect(NetworkConnection conn)
        {
            onClientDisconnect.Invoke(conn);
        }

        public void RegisterClientDisconnectHandler(UnityAction<NetworkConnection> handler)
        {
            onClientDisconnect.AddListener(handler);
        }

        public void OnClientAddError()
        {
            onClientAddError.Invoke();
        }

        public void RegisterClientErrorHandler(UnityAction handler)
        {
            onClientAddError.AddListener(handler);
        }

        public void OnStartServer()
        {
            onStartServer.Invoke();
        }

        public void RegisterStartServerHandler(UnityAction handler)
        {
            onStartServer.AddListener(handler);
        }

        public void OnStopServer()
        {
            onStopServer.Invoke();
        }

        public void RegisterStopServerHandler(UnityAction handler)
        {
            onStopServer.AddListener(handler);
        }
       
        public void OnPlayerAdded()
        {
            onAddPlayer.Invoke();
        }

        public void RegisterPlayerAddedHandler(UnityAction handler)
        {
            onAddPlayer.AddListener(handler);
        }

        public void OnPlayerRemoved()
        {
            onRemovePlayer.Invoke();
        }

        public void RegisterPlayerRemovedHandler(UnityAction handler)
        {
            onRemovePlayer.AddListener(handler);
        }

        public void OnMatchCreate()
        {
            onMatchCreate.Invoke();
        }

        public void RegisterMatchCreatedHandler(UnityAction handler)
        {
            onMatchCreate.AddListener(handler);
        }

        public void OnMatchJoined()
        {
            onMatchJoined.Invoke();
        }

        public void RegisterMatchJoinedHandler(UnityAction handler)
        {
            onMatchJoined.AddListener(handler);
        }

        public void MatchListRefresh(List<MatchInfoSnapshot> l)
        {
            onMatchList.Invoke(l);
        }

        public void RegisterMatchListHandler(UnityAction<List<MatchInfoSnapshot>> handler)
        {
            onMatchList.AddListener(handler);
        }
        #endregion
    }
}