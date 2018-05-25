using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSGO_DLV.Networking
{
    public class LeaveGame : MonoBehaviour
    {
        public void Leave()
        {
            GameNetworkManager.Manager.ResetLobbyScene();
            GameNetworkManager.Manager.LeaveGame();
        }
    }
}