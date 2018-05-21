using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using CSGO_DLV.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

namespace CSGO_DLV.Menu
{
    public class ServerEntry : MonoBehaviour
    {
        [SerializeField]
        private Text serverName;
        [SerializeField]
        private Text availableSlots;
        [SerializeField]
        private Button joinButon;

        private LobbyHook lobbyHook;
        private NetworkID matchId;

        private void Awake()
        {
            lobbyHook = LobbyHook.Hook;
        }

        public void ExtractInfo(MatchInfoSnapshot info)
        {
            serverName.text = info.name;
            availableSlots.text = info.currentSize + "/" + info.maxSize + "Players";
            matchId = info.networkId;
            joinButon.onClick.AddListener(JoinServer);
        }

        private void JoinServer()
        {
            GameNetworkManager.Manager.JoinMatch(matchId);
        }
    }
}