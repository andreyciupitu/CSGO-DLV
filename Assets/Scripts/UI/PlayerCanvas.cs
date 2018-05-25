using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace CSGO_DLV.HUD
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
        [SerializeField]
        Text timeText;

        private void Awake()
        {
            //Ensure there is only one PlayerCanvas
            if (canvas == null)
                canvas = this;
            else if (canvas != this)
                Destroy(gameObject);
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

        public void SetTime(int timeLeft)
        {
            timeText.text = timeLeft.ToString();
        }
    }
}