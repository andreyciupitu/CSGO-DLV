using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSGO_DLV.NetworkPlayer;
using System.Linq;

namespace CSGO_DLV.Game
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance = null;
        [SerializeField]
        private int maxScore;
        [SerializeField]
        private int pointsPerKill;
        [SerializeField]
        private float matchDuration;
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
        }

        private void Start()
        {
            //StartCoroutine(GameCountdown());
        }
        
        public int UpdateScore(int score)
        {
            return score += pointsPerKill;
        }

        public bool GameWon(int score)
        {
            return score >= maxScore;
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
/*
        public IEnumerator GameCountdown()
        {
            float remainingTime = matchDuration;
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
        }*/
    }
}
