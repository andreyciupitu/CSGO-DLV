using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CSGO_DLV.Networking;

namespace CSGO_DLV.Menu
{
    public class PlayerList : MonoBehaviour
    {
        public GameObject entryPrefab;

        private LobbyHook lobbyHook;
        private List<GameObject> entries = new List<GameObject>();

        private void Awake()
        {
            lobbyHook = LobbyHook.Hook;
            lobbyHook.RegisterPlayerAddedHandler(Refresh);
            lobbyHook.RegisterPlayerRemovedHandler(Refresh);
        }

        /// <summary>
        /// Obtains all the players in the lobby
        /// and displays their info
        /// </summary>
        public void Refresh()
        {
            NetworkLobbyPlayer[] players = GameNetworkManager.Manager.lobbySlots;

            // Remove the old entries
            foreach (GameObject entry in entries)
                Destroy(entry);
            entries.Clear();

            // Add an entry for each player
            foreach (NetworkLobbyPlayer p in players)
            {
                if (p != null)
                {
                    GameObject entry = Instantiate(entryPrefab, transform);

                    // Bind the entry to the player
                    (p as GameLobbyPlayer).Entry = entry.GetComponent<PlayerEntry>();

                    // Add the entry to the Content window
                    entry.transform.SetParent(transform);
                    entries.Add(entry);
                }
            }
        }
    }
}
