using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

namespace CSGO_DLV.Player
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

        GameObject mainCamera;

        void Start()
        {
            mainCamera = Camera.main.gameObject;

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
                // added
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

        private void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            mainCamera.SetActive(true);
        }
    }

}