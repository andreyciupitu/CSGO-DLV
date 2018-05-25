using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSGO_DLV.Game
{
    public class GameSettings : MonoBehaviour
    {
        public enum GameMode
        {
            DEATHMATCH, LAST_MAN_STANDING, CTF
        }

        private static GameSettings instance = null;

        // Singleton instance
        public static GameSettings Settings
        {
            get
            {
                return instance;
            }
        }
        public GameMode Mode { get; set; }
        public int PointsPerKill { get; set; }
        public int MatchDuration { get; set; }
        public int MaxScore { get; set; }

        private void Awake()
        {
            // Sort of Singleton Pattern - keep the new instance
            // This is so the events & references set in the editor
            // don't get lost when the other instance is destroyed
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Destroy(instance.gameObject); // Keep only one instance
                instance = this;
            }

            // Keep this between scenes
            DontDestroyOnLoad(gameObject);
            ResetSettings();
        }

        /// <summary>
        /// Resets Game Settings back to default values
        /// </summary>
        public void ResetSettings()
        {
            PointsPerKill = 1;
            MaxScore = 10;
            MatchDuration = 300;
        }
    }
}