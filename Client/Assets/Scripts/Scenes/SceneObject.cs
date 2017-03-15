using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Scenes
{
    public class SceneObject : NetworkBehaviour
    {
        private GameObject _parent;
        private Vector3 _position;
        private Quaternion _rotation;
        bool _stayInWorldSpace;
        int _layer;

        public override void OnStartClient()
        {
            
        }

        public void Update()
        {
            var scene = FindObjectOfType<Scene>();
            _parent = scene.gameObject;
                transform.SetParent(_parent.transform, _stayInWorldSpace);
                if (!_stayInWorldSpace)
                {
                    transform.localPosition = _position;
                    transform.localRotation = _rotation;
                }
                else
                {
                    transform.position = _position;
                    transform.rotation = _rotation;
                }

                gameObject.layer = _layer;
                foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
                    transform.gameObject.layer = _layer;
        }

        [ClientRpc]
        public void RpcSetTransform(Vector3 position, Quaternion rotation, GameObject parent, bool stayInWorldSpace)
        {
            _parent = parent;
            _position = position;
            _rotation = rotation;
            _stayInWorldSpace = stayInWorldSpace;
        }

        [ClientRpc]
        public void RpcSetLayer(int layer)
        {
            _layer = layer;
        }
    }
}
