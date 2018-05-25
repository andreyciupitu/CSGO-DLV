using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CSGO_DLV.NetworkPlayer;
using CSGO_DLV.Networking;
using System.Linq;

namespace CSGO_DLV.Game
{
    public class GameController : NetworkBehaviour
    {
        private static GameController instance = null;
        
        private GameSettings settings;
        private List<Player> players;

        public static GameController Controller
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            players = new List<Player>();
            // Singleton Pattern
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Destroy(gameObject); // Keep only one instance
            }
            settings = GameSettings.Settings;
        }

        private void Start()
        {
            if (NetworkServer.active)
                StartCoroutine(GameCountdown());
        }
        
        public int UpdateScore(int score)
        {
            return score += settings.PointsPerKill;
        }

        public bool GameWon(int score)
        {
            return score >= settings.MaxScore;
        }

        public void RegisterPlayer(Player player)
        {
            players.Add(player);
        }

        public void UnregisterPlayer(Player player)
        {
            players.Remove(player);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        [Server]
        public IEnumerator GameCountdown()
        {
            float remainingTime = settings.MatchDuration;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {
                    floorTime = newFloorTime;

                    foreach (Player player in players)
                    {
                        if (player != null)
                        {
                            player.RpcGameCountdown(floorTime);
                        }
                    }
                }
            }

            foreach (Player player in players)
            {
                if (player != null)
                {
                    player.RpcGameCountdown(floorTime);
                }
            }
        }
    }
}
