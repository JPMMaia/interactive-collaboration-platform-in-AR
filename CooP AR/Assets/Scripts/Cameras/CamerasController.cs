using System.Linq;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Cameras
{
    public class CamerasController : Entity
    {
        #region Unity Editor
        public CameraItemView[] CameraItemViews;

        public bool EnableKeyboardInput = true;
        public KeyCode FreeCameraViewKey = KeyCode.Escape;
        public KeyCode RightViewKey = KeyCode.RightArrow;
        public KeyCode TopViewKey = KeyCode.UpArrow;
        public KeyCode FrontViewKey = KeyCode.DownArrow;
        public KeyCode OppositeViewKey = KeyCode.LeftAlt;
        #endregion

        public CameraViewType SelectedCamera
        {
            get
            {
                return _selectedCamera;
            }
            set
            {
                if (_selectedCameraItemView != null)
                    _selectedCameraItemView.Selected = false;

                _selectedCamera = value;
                CameraManager.SelectedCameraType = value;

                _selectedCameraItemView = GetCameraItemView(value);
                if (_selectedCameraItemView != null)
                {
                    _selectedCameraItemView.Selected = true;
                }
            }
        }
        public CameraManager CameraManager
        {
            get { return FindObjectOfType<CameraManager>(); }
        }

        private CameraViewType _selectedCamera;
        private CameraItemView _selectedCameraItemView;

        public void Awake()
        {
            SelectedCamera = CameraViewType.Free;
            CameraManager.SelectedCameraType = CameraViewType.Free;

            foreach (var cameraItemView in CameraItemViews)
                cameraItemView.OnClicked += CameraItemView_OnClicked;
        }

        public void Update()
        {
            if (!EnableKeyboardInput)
                return;

            if (Input.GetKeyDown(FreeCameraViewKey))
            {
                SelectedCamera = CameraViewType.Free;
            }
            else if (Input.GetKey(OppositeViewKey))
            {
                if (Input.GetKeyDown(RightViewKey))
                    SelectedCamera = CameraViewType.Left;

                if (Input.GetKeyDown(TopViewKey))
                    SelectedCamera = CameraViewType.Bottom;

                if (Input.GetKeyDown(FrontViewKey))
                    SelectedCamera = CameraViewType.Back;
            }
            else
            {
                if (Input.GetKeyDown(RightViewKey))
                    SelectedCamera = CameraViewType.Right;

                if (Input.GetKeyDown(TopViewKey))
                    SelectedCamera = CameraViewType.Top;

                if (Input.GetKeyDown(FrontViewKey))
                    SelectedCamera = CameraViewType.Front;
            }
        }

        private CameraItemView GetCameraItemView(CameraViewType type)
        {
            var query = from camera in CameraItemViews
                        where camera.Type == type
                        select camera;

            return query.First();
        }

        private void CameraItemView_OnClicked(CameraItemView sender, CameraItemView.ViewEventArgs eventArgs)
        {
            SelectedCamera = eventArgs.Type;
        }
    }
}
