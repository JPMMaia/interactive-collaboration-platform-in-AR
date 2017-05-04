using CollaborationEngine.RuntimeGizmo;
using UnityEngine;

namespace CollaborationEngine.Camera
{
    public class TransformGizmoManager : MonoBehaviour
    {
        #region Members
        private static TransformGizmoManager _instance;
        private Transform _target;
        private UnityEngine.Camera _selectedCamera;
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
                    if(_target == null)
                        transformGizmo.UnselectGameObject();
                    else
                        transformGizmo.SelectGameObject(_target);
                }
            }
        }
        public UnityEngine.Camera SelectedCamera
        {
            get { return _selectedCamera; }
            set
            {
                if(_selectedCamera != null)
                    _selectedCamera.gameObject.GetComponent<TransformGizmo>().UnselectGameObject();

                _selectedCamera = value;

                if (_selectedCamera != null && _target != null)
                    _selectedCamera.gameObject.GetComponent<TransformGizmo>().SelectGameObject(_target);
            }
        }
        #endregion
    }
}
