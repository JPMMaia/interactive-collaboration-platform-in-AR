using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Objects
{
    public abstract class SceneObject : NetworkBehaviour
    {
        private GameObject _parent;
        private Vector3 _position;
        private Quaternion _rotation;
        private bool _stayInWorldSpace;
        private int _layer;
        private bool _dirty = true;

        protected virtual void Update()
        {
            if (!_dirty)
                return;

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
            foreach (Transform childTransform in gameObject.GetComponentsInChildren<Transform>(true))
                childTransform.gameObject.layer = _layer;

            _dirty = false;
        }

        [ClientRpc]
        public void RpcSetTransform(Vector3 position, Quaternion rotation, bool stayInWorldSpace)
        {
            _position = position;
            _rotation = rotation;
            _stayInWorldSpace = stayInWorldSpace;
            _dirty = true;
        }

        [ClientRpc]
        public void RpcSetLayer(int layer)
        {
            _layer = layer;
            _dirty = true;
        }
    }
}
