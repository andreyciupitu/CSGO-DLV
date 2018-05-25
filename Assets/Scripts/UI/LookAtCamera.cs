using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSGO_DLV.HUD
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Update()
        {
            Camera mainCamera = Camera.main; // Nasty shit happening here

            if (mainCamera == null)
                return;

            // Make the UI element look at the camera
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
    }
}
