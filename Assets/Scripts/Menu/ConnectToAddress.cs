﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CSGO_DLV.Networking;

namespace CSGO_DLV.Menu
{
    public class ConnectToAddress : MonoBehaviour
    {
        private InputField input;

        private void Awake()
        {
            input = GetComponentInChildren<InputField>();
        }

        public void Connect()
        {
            GameNetworkManager.Manager.JoinLanGame(input.text);
        }
    }
}
