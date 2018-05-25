using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CSGO_DLV.Networking;
using UnityEngine.Networking.Match;

namespace CSGO_DLV.Menu
{
    public class ServerList : MonoBehaviour
    {
        [SerializeField]
        private int pageSize = 4;
        [SerializeField]
        private GameObject entryPrefab;

        private LobbyHook lobbyHook;
        private List<GameObject> entries = new List<GameObject>();
        private int currentPage;

        private void Awake()
        {
            currentPage = 0;
            lobbyHook = LobbyHook.Hook;
            lobbyHook.RegisterMatchListHandler(Refresh);
        }

        private void OnEnable()
        {
            currentPage = 0;
            GameNetworkManager.Manager.GetMatchPage(currentPage, pageSize);
        }

        /// <summary>
        /// Displays the new list of available servers
        /// </summary>
        public void Refresh(List<MatchInfoSnapshot> matches)
        {
            // Remove the old entries
            foreach (GameObject entry in entries)
                Destroy(entry);
            entries.Clear();

            if (matches.Count == 0)
                currentPage = 0;

            // Add an entry for each match
            foreach (MatchInfoSnapshot m in matches)
            {
                if (m != null)
                {
                    GameObject entry = Instantiate(entryPrefab, transform);

                    // Extract info from snapshot
                    entry.GetComponent<ServerEntry>().ExtractInfo(m);
                    
                    // Add the entry to the Content window
                    entry.transform.SetParent(transform);
                    entries.Add(entry);
                }
            }
        }

        #region Matchmaker functions
        public void GetCurrentPage()
        {
            GameNetworkManager.Manager.GetMatchPage(currentPage, pageSize);
        }

        public void GetNextPage()
        {
            GameNetworkManager.Manager.GetMatchPage(++currentPage, pageSize);
        }
        #endregion
    }
}
