using UnityEngine;

namespace CollaborationEngine.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        #region Unity Editor
        public FreeCamera FreeCamera;
        public SideCamera RightCamera;
        public SideCamera LeftCamera;
        public SideCamera TopCamera;
        public SideCamera BottomCamera;
        public SideCamera FrontCamera;
        public SideCamera BackCamera;
        #endregion

        #region Properties

        public ICamera SelectedCamera
        {
            get
            {
                return _selectedCamera;
            }
            private set
            {
                if (_selectedCamera != null)
                {
                    _selectedCamera.Selected = false;
                    _selectedCamera.UnityCamera.gameObject.SetActive(false);
                    TransformGizmoManager.Instance.SelectedCamera = null;
                }

                _selectedCamera = value;

                if (_selectedCamera != null)
                {
                    _selectedCamera.Selected = true;
                    _selectedCamera.UnityCamera.gameObject.SetActive(true);
                    if (TransformGizmoManager.Instance != null)
                        TransformGizmoManager.Instance.SelectedCamera = _selectedCamera.UnityCamera;
                }
            }
        }
        public CameraViewType SelectedCameraType
        {
            get { return _selectedCameraType; }
            set
            {
                _selectedCameraType = value;

                switch (value)
                {
                    case CameraViewType.Right:
                        SelectedCamera = RightCamera;
                        break;

                    case CameraViewType.Top:
                        SelectedCamera = TopCamera;
                        break;

                    case CameraViewType.Front:
                        SelectedCamera = FrontCamera;
                        break;

                    case CameraViewType.Left:
                        SelectedCamera = LeftCamera;
                        break;

                    case CameraViewType.Bottom:
                        SelectedCamera = BottomCamera;
                        break;

                    case CameraViewType.Back:
                        SelectedCamera = BackCamera;
                        break;

                    default:
                        SelectedCamera = FreeCamera;
                        break;
                }
            }
        }

        #endregion

        #region Members
        private CameraViewType _selectedCameraType;
        private ICamera _selectedCamera;
        #endregion
    }
}
