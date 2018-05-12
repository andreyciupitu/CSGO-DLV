using UnityEngine;
using UnityEngine.UI;
using CSGO_DLV.Networking;

namespace CSGO_DLV.Menu
{
    public class PlayerCounter : MonoBehaviour
    {
        public void UpdateText()
        {
            GetComponent<Text>().text = GameNetworkManager.Manager.PlayerCount
                                        + "/" + GameNetworkManager.Manager.maxPlayers + " Players";
        }
    }
}
