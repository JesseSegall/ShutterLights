using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class CamSense : MonoBehaviour
    {
        public GameObject player;
        private ThirdPersonController thirdPersonController;

        private void Awake()
        {
            if (player != null)
            {
                thirdPersonController = player.GetComponentInChildren<ThirdPersonController>();
            }
            else
            {
                Debug.Log("Player is missing for menu camera sens. slider");
            }

            if (thirdPersonController == null)
            {
                Debug.Log("thirdPersonController is missing for menu camera sens. slider");
            }

        }

        public void SliderChange(float value)
        {
            // default value is 0.5 resulting in cam sens of 2
            // range is 1 - 3
            thirdPersonController.cameraSensitivity = 1.0f + value * 2.0f;
            Debug.Log("Cam sens is now " + thirdPersonController.cameraSensitivity);
        }

    }

}
