using System.IO;
using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Cameras
{
    [AddComponentMenu("Camera-Control/Free Camera")]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class FreeCamera : MonoBehaviour, ICamera, ISerializable
    {
        #region Unity Editor
        public float MovementSensibility = 0.1f;
        public float RotationSensibility = 1.0f;
        public float MouseSensibility = 10.0f;
        #endregion

        #region Properties
        public Vector3 LocalRight
        {
            get
            {
                var localRight = _rotationMatrix.GetColumn(0);
                return new Vector3(localRight.x, localRight.y, localRight.z);
            }
        }
        public Vector3 LocalForward
        {
            get
            {
                var localForward = _rotationMatrix.GetColumn(2);
                return new Vector3(localForward.x, localForward.y, localForward.z);
            }
        }
        public bool Selected { get; set; }
        public UnityEngine.Camera UnityCamera
        {
            get { return GetComponent<UnityEngine.Camera>(); }
            set
            {
            }
        }
        #endregion

        #region Members
        private Transform _transform;
        private Vector3 _position;
        private Quaternion _rotationQuaternion;
        private Matrix4x4 _rotationMatrix;
        private bool _dirty;
        #endregion

        public void Awake()
        {
            gameObject.SetActive(false);

            _transform = GetComponent<Transform>();
            _position = _transform.position;
            _rotationQuaternion = _transform.rotation;
            _rotationMatrix = Matrix4x4.TRS(Vector3.zero, _rotationQuaternion, Vector3.zero);
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
                MoveForward(movementSensibility);
            if (Input.GetKey(KeyCode.S))
                MoveForward(-movementSensibility);
            if (Input.GetKey(KeyCode.A))
                MoveRight(-movementSensibility);
            if (Input.GetKey(KeyCode.D))
                MoveRight(movementSensibility);

            var rotationSensibility = RotationSensibility;
            if (Input.GetKey(KeyCode.Q))
                RotateWorldZ(rotationSensibility);
            if (Input.GetKey(KeyCode.E))
                RotateWorldZ(-rotationSensibility);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                var mouseSensibility = MouseSensibility;
                var mouseDeltaX = Input.GetAxis("Mouse X");
                var mouseDeltaY = Input.GetAxis("Mouse Y");

                RotateWorldY(mouseDeltaX * mouseSensibility);
                RotateWorldX(-mouseDeltaY * mouseSensibility);
            }

            if (_dirty)
            {
                // CreateStep the camera rotation matrix:
                _rotationMatrix = Matrix4x4.TRS(Vector3.zero, _rotationQuaternion, Vector3.one);

                _transform.position = _position;
                _transform.rotation = _rotationQuaternion;

                _dirty = false;
            }
        }

        private void Move(Vector3 axis, float scalar)
        {
            // Calculate the translation:
            var translation = axis * scalar;

            // Apply the translation:
            _position += translation;
            _dirty = true;
        }
        private void MoveRight(float scalar)
        {
            Move(LocalRight, scalar);
        }
        private void MoveForward(float scalar)
        {
            Move(LocalForward, scalar);
        }

        private void Rotate(Vector3 axis, float degrees)
        {
            // Calculate the rotation arround the axis:
            var rotation = Quaternion.AngleAxis(degrees, axis);

            // Apply the rotation:
            _rotationQuaternion = _rotationQuaternion * rotation;
            _dirty = true;
        }
        private void RotateWorldX(float degrees)
        {
            // Calculate the rotation arround the world X-axis:
            var worldXAxis = new Vector3(1.0f, 0.0f, 0.0f);
            Rotate(worldXAxis, degrees);
        }
        private void RotateWorldY(float degrees)
        {
            // Calculate the rotation arround the world Y-axis:
            var worldYAxis = new Vector3(0.0f, 1.0f, 0.0f);
            Rotate(worldYAxis, degrees);
        }
        private void RotateWorldZ(float degrees)
        {
            // Calculate the rotation arround the world Z-axis:
            var worldZAxis = new Vector3(0.0f, 0.0f, 1.0f);
            Rotate(worldZAxis, degrees);
        }

        public void Serialize(BinaryWriter writer)
        {
            {
                writer.Write(transform.localPosition.x);
                writer.Write(transform.localPosition.y);
                writer.Write(transform.localPosition.z);
            }

            {
                writer.Write(transform.localRotation.x);
                writer.Write(transform.localRotation.y);
                writer.Write(transform.localRotation.z);
            }

            writer.Write(MovementSensibility);
            writer.Write(RotationSensibility);
            writer.Write(MouseSensibility);
        }
        public void Deserialize(BinaryReader reader)
        {
            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                transform.localPosition = new Vector3(x, y, z);
            }

            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                var w = reader.ReadSingle();
                transform.localRotation = new Quaternion(x, y, z, w);
            }

            MovementSensibility = reader.ReadSingle();
            RotationSensibility = reader.ReadSingle();
            MouseSensibility = reader.ReadSingle();
        }
    }
}