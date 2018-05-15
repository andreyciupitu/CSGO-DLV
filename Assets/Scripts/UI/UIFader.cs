using UnityEngine;
using System.Collections;

namespace CSGO_DLV.NetworkPlayer
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFader : MonoBehaviour
    {
        [SerializeField]
        float fadeSpeed = 1f;              // The amount the alpha of the UI elements changes per second.
        [SerializeField]
        CanvasGroup groupToFade;           // All the groups of UI elements that will fade in and out.
        [SerializeField]
        bool startVisible;                 // Should the UI elements be visible to start?
        [SerializeField]
        bool startWithFade;                // Should the UI elements begin fading with they start up

        bool visible;                      // Whether the UI elements are currently visible.


        void Reset()
        {
            //Attempt to grab the CanvasGroup on this object
            groupToFade = GetComponent<CanvasGroup>();
        }

        void Start()
        {
            //If the object should start visible, set it to be visible. Otherwise, set it invisible
            if (startVisible)
                SetVisible();
            else
                SetInvisible();

            //If there shouldn't be any initial fade, leave this method
            if (!startWithFade)
                return;

            //If the object is currently visible, fade out. Otherwise fade in
            if (visible)
                StartFadeOut();
            else
                StartFadeIn();
        }

        #region Fade API
        public void StartFadeIn()
        {
            StartCoroutine(FadeIn());
        }

        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }

        public void Flash()
        {
            StartCoroutine(ProcessFlash());
        }

        // These functions are used if fades are required to be instant.
        public void SetVisible()
        {
            groupToFade.alpha = 1f;
            visible = true;
        }


        public void SetInvisible()
        {
            groupToFade.alpha = 0f;
            visible = false;
        }
        #endregion

        #region Coroutines
        IEnumerator FadeIn()
        {
            // Fading needs to continue until the group is completely faded in
            while (groupToFade.alpha < 1f)
            {
                //Increase the alpha
                groupToFade.alpha += fadeSpeed * Time.deltaTime;
                //Wait a frame
                yield return null;
            }

            // Since everthing has faded in now, it is visible.
            visible = true;
        }

        IEnumerator FadeOut()
        {
            while (groupToFade.alpha > 0f)
            {
                groupToFade.alpha -= fadeSpeed * Time.deltaTime;

                yield return null;
            }

            visible = false;
        }

        IEnumerator ProcessFlash()
        {
            yield return StartCoroutine(FadeIn());
            yield return StartCoroutine(FadeOut());
        }

        #endregion
    }
}