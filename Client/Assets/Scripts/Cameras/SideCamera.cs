using System.IO;
using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class SideCamera : MonoBehaviour, ICamera, ISerializable
    {
        public enum ViewDirection
        {
            Right, Top, Front,
            Left, Bottom, Back
        }

        #region Unity Editor
        public ViewDirection View = ViewDirection.Front;
        public float MovementSensibility = 1.0f;
        public float MouseWheelSensibility = 10.0f;
        #endregion

        #region Properties
        public Vector3 LocalRight { get; private set; }
        public Vector3 LocalUp { get; private set; }
        public Vector3 LocalForward { get; private set; }
        public bool Selected { get; set; }
        public Camera UnityCamera
        {
            get { return GetComponent<Camera>(); }
        }
        public CameraViewType CameraType
        {
            get { return (CameraViewType) View; }
        }
        #endregion

        #region Members
        private Camera _camera;
        private Transform _transform;
        #endregion

        public void Awake()
        {
            gameObject.SetActive(false);

            _camera = GetComponent<Camera>();
            _camera.orthographic = true;

            switch (View)
            {
                case ViewDirection.Right:
                    LocalRight = Vector3.forward;
                    LocalUp = Vector3.up;
                    LocalForward = Vector3.left;
                    break;
                case ViewDirection.Top:
                    LocalRight = Vector3.right;
                    LocalUp = Vector3.forward;
                    LocalForward = Vector3.down;
                    break;
                case ViewDirection.Front:
                    LocalRight = Vector3.right;
                    LocalUp = Vector3.up;
                    LocalForward = Vector3.forward;
                    break;

                case ViewDirection.Left:
                    LocalRight = Vector3.back;
                    LocalUp = Vector3.up;
                    LocalForward = Vector3.right;
                    break;
                case ViewDirection.Bottom:
                    LocalRight = Vector3.right;
                    LocalUp = Vector3.back;
                    LocalForward = Vector3.up;
                    break;
                case ViewDirection.Back:
                    LocalRight = Vector3.left;
                    LocalUp = Vector3.up;
                    LocalForward = Vector3.back;
                    break;
            }

            _transform = GetComponent<Transform>();
            _transform.LookAt(_transform.position + LocalForward, LocalUp);
        }

        public void Update()
        {
            // If using UI:
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            if (!Selected)
                return;

            var movementSensibility = MovementSensibility;
            if (Input.GetKey(KeyCode.W))
                MoveUp(movementSensibility);
            if (Input.GetKey(KeyCode.S))
                MoveUp(-movementSensibility);
            if (Input.GetKey(KeyCode.A))
                MoveRight(-movementSensibility);
            if (Input.GetKey(KeyCode.D))
                MoveRight(movementSensibility);

            var mouseWheelInput = Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensibility;
            if (Mathf.Abs(mouseWheelInput) > Mathf.Epsilon)
                UnityCamera.orthographicSize -= mouseWheelInput;
        }

        private void Move(Vector3 axis, float scalar)
        {
            // Calculate the translation:
            var translation = axis * scalar;

            // Apply the translation:
            transform.position += translation;
        }
        private void MoveRight(float scalar)
        {
            Move(LocalRight, scalar);
        }
        private void MoveUp(float scalar)
        {
            Move(LocalUp, scalar);
        }

        public void Serialize(BinaryWriter writer)
        {
            {
                writer.Write(transform.localPosition.x);
                writer.Write(transform.localPosition.y);
                writer.Write(transform.localPosition.z);
            }

            writer.Write(UnityCamera.orthographicSize);
            writer.Write(MovementSensibility);
            writer.Write(MouseWheelSensibility);
        }
        public void Deserialize(BinaryReader reader)
        {
            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                transform.localPosition = new Vector3(x, y, z);
            }

            UnityCamera.orthographicSize = reader.ReadSingle();
            MovementSensibility = reader.ReadSingle();
            MouseWheelSensibility = reader.ReadSingle();
        }
    }
}
