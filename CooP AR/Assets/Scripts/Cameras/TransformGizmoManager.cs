using System;
using CollaborationEngine.RuntimeGizmo;
using UnityEngine;

namespace CollaborationEngine.Cameras
{
    public class TransformGizmoManager : MonoBehaviour
    {
        #region Events
        public class TransformEventArgs : EventArgs
        {
            public Transform Transform { get; private set; }

            public TransformEventArgs(Transform transform)
            {
                Transform = transform;
            }
        }

        public event EventHandler<TransformEventArgs> OnTargetTransformChanged;
        #endregion

        #region Unity Editor
        public TransformGizmo[] TransformGizmos;
        #endregion

        #region Members
        private static TransformGizmoManager _instance;
        private Transform _target;
        private Camera _selectedCamera;
        #endregion

        #region Properties
        public static TransformGizmoManager Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType(typeof(TransformGizmoManager)) as TransformGizmoManager;

                return _instance;
            }
        }
        public Transform Target
        {
            get { return _target; }
            set
            {
                _target = value;

                if (_selectedCamera != null)
                {
                    var transformGizmo = _selectedCamera.gameObject.GetComponent<TransformGizmo>();
                    if (_target != null)
                    {
                        transformGizmo.SelectGameObject(_target);
                    }
                    else
                    {
                        transformGizmo.UnselectGameObject();
                    }
                }
            }
        }
        public Camera SelectedCamera
        {
            get { return _selectedCamera; }
            set
            {
                if (_selectedCamera != null)
                {
                    var transformGizmo = _selectedCamera.gameObject.GetComponent<TransformGizmo>();
                    transformGizmo.UnselectGameObject();
                }

                _selectedCamera = value;

                if (_selectedCamera != null && _target != null)
                {
                    var transformGizmo = _selectedCamera.gameObject.GetComponent<TransformGizmo>();
                    transformGizmo.SelectGameObject(_target);
                }
            }
        }

        #endregion

        public void Awake()
        {
            foreach (var transformGizmo in TransformGizmos)
            {
                transformGizmo.OnTransformChanged += TransformGizmo_OnTransformChanged;
            }
        }

        #region Event Handlers
        private void TransformGizmo_OnTransformChanged(object sender, EventArgs eventArgs)
        {
            if (sender != null && (TransformGizmo) sender != _selectedCamera.gameObject.GetComponent<TransformGizmo>())
                return;

            if (OnTargetTransformChanged != null)
                OnTargetTransformChanged(this, new TransformEventArgs(Target));
        }
        #endregion
    }
}
