using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using CSGO_DLV.Game;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

namespace CSGO_DLV.NetworkPlayer
{

    public class Player : NetworkBehaviour
    {
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

            EnablePlayer();
        }

        void DisablePlayer()
        {
            if (isLocalPlayer)
            {
                mainCamera.SetActive(true);
                // added
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
        
        public void Update()
        {
            if (!NetworkManager.singleton.IsClientConnected())
                mainCamera.SetActive(true);
        }

        private void OnDestroy()
        {
            gameController.UnregisterPlayer(this);
        }

        private void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            mainCamera.SetActive(true);
        }
    }

}