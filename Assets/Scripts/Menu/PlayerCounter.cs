using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CSGO_DLV.Networking;

namespace CSGO_DLV.Menu
{
    public class PlayerCounter : MonoBehaviour
    {
        public void UpdateText()
        {
            int playerCount = 0;
            NetworkLobbyPlayer[] slots = GameNetworkManager.Manager.lobbySlots;

            foreach (NetworkLobbyPlayer p in slots)
            {
                if (p != null)
                    playerCount++;
            }

            GetComponent<Text>().text = playerCount + "/" + GameNetworkManager.Manager.maxPlayers + " Players";
        }
    }
}
