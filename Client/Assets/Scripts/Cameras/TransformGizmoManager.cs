using System;
using CollaborationEngine.Objects;
using CollaborationEngine.RuntimeGizmo;
using UnityEngine;

namespace CollaborationEngine.Cameras
{
    public class TransformGizmoManager : MonoBehaviour
    {
        #region Events
        public event EventHandler OnTargetTransformChanged;
        #endregion

        #region Members
        private static TransformGizmoManager _instance;
        private SceneObject _target;
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
        public SceneObject Target
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
                        transformGizmo.SelectGameObject(_target.GameObject.transform);
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
                    transformGizmo.OnTransformChanged -= TransformGizmo_OnTransformChanged;
                }


                _selectedCamera = value;

                if (_selectedCamera != null && _target != null)
                {
                    var transformGizmo = _selectedCamera.gameObject.GetComponent<TransformGizmo>();
                    transformGizmo.SelectGameObject(_target.GameObject.transform);
                    transformGizmo.OnTransformChanged += TransformGizmo_OnTransformChanged;
                }
            }
        }

        #endregion

        #region Event Handlers
        private void TransformGizmo_OnTransformChanged(object sender, EventArgs eventArgs)
        {
            if (OnTargetTransformChanged != null)
                OnTargetTransformChanged(this, EventArgs.Empty);
        }
        #endregion
    }
}
