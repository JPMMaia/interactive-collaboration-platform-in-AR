using System;
using UnityEngine;

namespace Assets.Scripts.Assets.Scripts.Client
{
    public class ClientController : MonoBehaviour
    {
        public Camera Camera;
        public int SceneLayer = 256;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                ToggleSceneVisibility();
        }

        public void ToggleSceneVisibility()
        {
            Camera.cullingMask ^= SceneLayer;
        }
    }
}
