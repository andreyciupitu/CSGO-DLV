using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.UI;
using CSGO_DLV.Game;
using CSGO_DLV.HUD;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

namespace CSGO_DLV.NetworkPlayer
{

    public class Player : NetworkBehaviour
    {
        [SyncVar(hook = "OnColorChanged")]
        public Color color;
        
        [SyncVar(hook = "OnNameChanged")]
        public string playerName;

        [SerializeField]
        private Text nameText;
        [SerializeField]
        ToggleEvent onToggleShared;
        [SerializeField]
        ToggleEvent onToggleLocal;
        [SerializeField]
        ToggleEvent onToggleRemote;
        [SerializeField]
        float respawnTime = 3f;

        private GameController gameController;
        GameObject mainCamera;

        void Start()
        {
            mainCamera = Camera.main.gameObject;
            gameController = GameController.Controller;
            gameController.RegisterPlayer(this);

            OnNameChanged(playerName);
            OnColorChanged(color);
            EnablePlayer();
        }

        void DisablePlayer()
        {
            if (isLocalPlayer)
            {
                mainCamera.SetActive(true);
                PlayerCanvas.canvas.HideReticule();
            }
            onToggleShared.Invoke(false);

            if (isLocalPlayer)
                onToggleLocal.Invoke(false);
            else
                onToggleRemote.Invoke(false);
        }

        void EnablePlayer()
        {
            if (isLocalPlayer)
            {
                mainCamera.SetActive(false);
                PlayerCanvas.canvas.Initialize();
            }
            onToggleShared.Invoke(true);

            if (isLocalPlayer)
                onToggleLocal.Invoke(true);
            else
                onToggleRemote.Invoke(true);
        }

        public void Die()
        {
            if (isLocalPlayer)
            {
                PlayerCanvas.canvas.WriteGameStatusText("You Died!");
            }

            DisablePlayer();
            Invoke("Respawn", respawnTime);
        }

        void Respawn()
        {
            if (isLocalPlayer)
            {
                Transform spawn = NetworkManager.singleton.GetStartPosition();
                transform.position = spawn.position;
                transform.rotation = spawn.rotation;
            }
            EnablePlayer();
        }

        private void OnDestroy()
        {
            gameController.UnregisterPlayer(this);
        }

        public void RpcGameCountdown(int time)
        {
            PlayerCanvas.canvas.SetTime(time);
        }

        private void OnNameChanged(string name)
        {
            playerName = name;
            gameObject.name = name;
            GetComponentInChildren<Text>(true).text = name;
        }

        private void OnColorChanged(Color color)
        {
            this.color = color;
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++) 
            {
                renderers[i].material.color = color;
            }

        }
    }

}