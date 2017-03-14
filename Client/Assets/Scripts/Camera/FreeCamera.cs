using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Free Camera")]
public class FreeCamera : MonoBehaviour
{
    public float MovementSensibility = 0.1f;
    public float RotationSensibility = 1.0f;
    public float MouseSensibility = 10.0f;

    public Vector3 LocalRight
    {
        get
        {
            var localRight = m_rotationMatrix.GetColumn(0);
            return new Vector3(localRight.x, localRight.y, localRight.z);
        }
    }
    public Vector3 LocalForward
    {
        get
        {
            var localForward = m_rotationMatrix.GetColumn(2);
            return new Vector3(localForward.x, localForward.y, localForward.z);
        }
    }

    private Transform m_transform;
    private Vector3 m_position;
    private Quaternion m_rotationQuaternion;
    private Matrix4x4 m_rotationMatrix;
    private Matrix4x4 m_viewMatrix;
    private bool m_dirty;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        m_position = m_transform.position;
        m_rotationQuaternion = m_transform.rotation;
        m_rotationMatrix = Matrix4x4.TRS(Vector3.zero, m_rotationQuaternion, Vector3.zero);
    }

    private void Update()
    {
        float movementSensibility = MovementSensibility;
        if (Input.GetKey(KeyCode.W))
            MoveForward(movementSensibility);
        if (Input.GetKey(KeyCode.S))
            MoveForward(-movementSensibility);
        if (Input.GetKey(KeyCode.A))
            MoveRight(-movementSensibility);
        if (Input.GetKey(KeyCode.D))
            MoveRight(movementSensibility);

        float rotationSensibility = RotationSensibility;
        if (Input.GetKey(KeyCode.Q))
            RotateWorldZ(rotationSensibility);
        if (Input.GetKey(KeyCode.E))
            RotateWorldZ(-rotationSensibility);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            float mouseSensibility = MouseSensibility;
            var mouseDeltaX = Input.GetAxis("Mouse X");
            var mouseDeltaY = Input.GetAxis("Mouse Y");

            RotateWorldY(mouseDeltaX * mouseSensibility);
            RotateWorldX(-mouseDeltaY * mouseSensibility);
        }

        if (m_dirty)
        {
            // Create the camera translation matrix:
            var translationMatrix = Matrix4x4.TRS(m_position, Quaternion.identity, Vector3.one);

            // Create the camera rotation matrix:
            m_rotationMatrix = Matrix4x4.TRS(Vector3.zero, m_rotationQuaternion, Vector3.one);
            
            // Build view matrix:
            m_viewMatrix = translationMatrix * m_rotationMatrix;

            m_transform.position = m_position;
            m_transform.rotation = m_rotationQuaternion;

            m_dirty = false;
        }
    }

    private void Move(Vector3 axis, float scalar)
    {
        // Calculate the translation:
        var translation = axis * scalar;

        // Apply the translation:
        m_position += translation;
        m_dirty = true;
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
        m_rotationQuaternion = m_rotationQuaternion * rotation;
        m_dirty = true;
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
}