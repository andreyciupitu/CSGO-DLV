using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CSGO_DLV.Networking
{

    public class LobbyHook : MonoBehaviour
    {
        // TODO make these protected & add Invoke methods
        public UnityEvent OnLobbyStartHost;
        public UnityEvent<NetworkConnection> OnLobbyClientConnect;
        public UnityEvent<NetworkConnection> OnLobbyClientDisconnect;
        public UnityEvent<NetworkConnection> OnLobbyClientSceneChanged;

        public bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            return true;
        }
    }

}