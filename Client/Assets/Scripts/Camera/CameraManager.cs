using UnityEngine;

namespace CollaborationEngine.Camera
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

        public KeyCode FreeCameraViewKey = KeyCode.Escape;
        public KeyCode RightViewKey = KeyCode.RightArrow;
        public KeyCode TopViewKey = KeyCode.UpArrow;
        public KeyCode FrontViewKey = KeyCode.DownArrow;
        public KeyCode OppositeViewKey = KeyCode.LeftAlt;

        #endregion

        #region Properties

        public ICamera SelectedCamera
        {
            get
            {
                return _selectedCamera;
            }
            set
            {
                if (_selectedCamera != null)
                {
                    _selectedCamera.Selected = false;
                    _selectedCamera.UnityCamera.gameObject.SetActive(false);
                }

                _selectedCamera = value;

                if (_selectedCamera != null)
                {
                    _selectedCamera.Selected = true;
                    _selectedCamera.UnityCamera.gameObject.SetActive(true);
                }
            }
        }

        #endregion

        #region Members
        private ICamera _selectedCamera;
        #endregion

        public void Start()
        {
            SelectedCamera = FreeCamera;
        }

        public void Update()
        {
            if(Input.GetKeyDown(FreeCameraViewKey))
            {
                SelectedCamera = FreeCamera;
            }
            else if (Input.GetKey(OppositeViewKey))
            {
                if (Input.GetKeyDown(RightViewKey))
                    SelectedCamera = LeftCamera;

                if (Input.GetKeyDown(TopViewKey))
                    SelectedCamera = BottomCamera;

                if (Input.GetKeyDown(FrontViewKey))
                    SelectedCamera = BackCamera;
            }
            else
            {
                if (Input.GetKeyDown(RightViewKey))
                    SelectedCamera = RightCamera;

                if (Input.GetKeyDown(TopViewKey))
                    SelectedCamera = TopCamera;

                if (Input.GetKeyDown(FrontViewKey))
                    SelectedCamera = FrontCamera;
            }
        }
    }
}
