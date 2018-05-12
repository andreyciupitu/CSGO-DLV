using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CSGO_DLV.Networking;
using System.Collections.Generic;
using System.Linq;

namespace CSGO_DLV.Menu
{ 
    public class PlayerEntry : MonoBehaviour
    {
        [SerializeField]
        private InputField input;
        [SerializeField]
        private Button ready;
        [SerializeField]
        private Dropdown dropdown;
        [SerializeField]
        private Button kickButton;

        private GameLobbyPlayer player;
        private string notReadyMsg = "";

        private void Awake()
        {
            // Kick button is only available for hosts
            if (!NetworkServer.active)
                kickButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Binds the entry to a GameLobbyPlayer
        /// </summary>
        /// <param name="p">The target player</param>
        public void SetToPlayer(GameLobbyPlayer p)
        {
            player = p;
            
            input.onEndEdit.AddListener(player.ChangeName);
            
            dropdown.ClearOptions();
            dropdown.AddOptions(p.Skins.Select(skin => skin.name).ToList());
            dropdown.onValueChanged.AddListener(player.ChangeSkin);

            kickButton.onClick.AddListener(OnKickPressed);
            
            ready.onClick.AddListener(LobbyHook.Hook.PlayerReady);

            // Setup for each player
            LocalPlayerSetup();
            UpdateInfo();
        }

        /// <summary>
        /// Enables UI elements for the local player
        /// </summary>
        public void LocalPlayerSetup()
        {
            notReadyMsg = player.isLocalPlayer? "Ready!" : "Not Ready!";
            input.interactable = player.isLocalPlayer;
            dropdown.interactable = player.isLocalPlayer;
            kickButton.gameObject.SetActive(!player.isLocalPlayer && NetworkServer.active);
        }

        /// <summary>
        /// Updates the displayed info
        /// </summary>
        public void UpdateInfo()
        {
            input.text = player.Name;

            dropdown.value = player.SkinNumber;

            ready.interactable = !player.readyToBegin && player.isLocalPlayer;            
            if (player.readyToBegin)
                ready.GetComponentInChildren<Text>().text = "Ready!";
            else
                ready.GetComponentInChildren<Text>().text = notReadyMsg;
        }

        private void OnKickPressed()
        {
            GameNetworkManager.Manager.KickPlayer(player);
        }
    }
}
