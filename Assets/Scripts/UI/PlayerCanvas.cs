using UnityEngine;
using UnityEngine.UI;

namespace CSGO_DLV.NetworkPlayer
    { 
    public class PlayerCanvas : MonoBehaviour
    {
        public static PlayerCanvas canvas;

        [Header("Component References")]
        [SerializeField]
        Image reticule;
        [SerializeField]
        UIFader damageImage;
        [SerializeField]
        Text gameStatusText;
        [SerializeField]
        Text healthValue;
        [SerializeField]
        Text killsValue;
        [SerializeField]
        private GameObject menu;
        /*
        [SerializeField] Text logText;
        [SerializeField] AudioSource deathAudio;
        */

        private void Awake()
        {
            //Ensure there is only one PlayerCanvas
            if (canvas == null)
                canvas = this;
            else if (canvas != this)
                Destroy(gameObject);
        }

        private void Reset()
        {
            //Find all of our resources
            reticule = GameObject.Find("Reticule").GetComponent<Image>();
            damageImage = GameObject.Find("DamagedFlash").GetComponent<UIFader>();
            gameStatusText = GameObject.Find("GameStatusText").GetComponent<Text>();
            healthValue = GameObject.Find("HealthValue").GetComponent<Text>();
            killsValue = GameObject.Find("KillsValue").GetComponent<Text>();
            /*
            logText = GameObject.Find("LogText").GetComponent<Text>();
            deathAudio = GameObject.Find("DeathAudio").GetComponent<AudioSource>();
            */
        }

        public void ShowMenu(bool active)
        {
            menu.SetActive(active);
            if (active)
                HideReticule();
            else
                Initialize();
        }

        public void Initialize()
        {
            reticule.enabled = true;
            gameStatusText.text = "";
        }

        public void HideReticule()
        {
            reticule.enabled = false;
        }

        public void FlashDamageEffect()
        {
            damageImage.Flash();
        }

        public void SetKills(int amount)
        {
            killsValue.text = amount.ToString();
        }
    
        public void SetHealth(int amount)
        {
            healthValue.text = amount.ToString();
        }
    
        public void WriteGameStatusText(string text)
        {
            gameStatusText.text = text;

            HideReticule();
        }
        /*
        public void WriteLogText(string text, float duration)
        {
            CancelInvoke();
            logText.text = text;
            Invoke("ClearLogText", duration);
        }

        void ClearLogText()
        {
            logText.text = "";
        }
        */
    }
}