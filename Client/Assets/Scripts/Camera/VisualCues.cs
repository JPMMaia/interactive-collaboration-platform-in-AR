using UnityEngine;

namespace CollaborationEngine.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class VisualCues : MonoBehaviour
    {
        public GameObject LeftArrow;
        public GameObject RightArrow;

        public void Awake()
        {
            _mainCamera = GetComponent<UnityEngine.Camera>();

            LeftArrow.transform.position = FromViewportToWorldSpace(new Vector3(0.1f, 0.5f, 0.5f));
            LeftArrow.SetActive(false);

            RightArrow.transform.position = FromViewportToWorldSpace(new Vector3(0.9f, 0.5f, 0.5f));
            RightArrow.SetActive(false);
        }

        public bool IsInFieldOfView(Vector3 viewportPosition)
        {
            return viewportPosition.z > 0 && viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;
        }

        public Vector3 FromViewportToWorldSpace(Vector3 viewportPosition)
        {
            return _mainCamera.ViewportToWorldPoint(viewportPosition);
        }
        public Vector3 FromWorldToViewportSpace(Vector3 worldPosition)
        {
            return _mainCamera.WorldToViewportPoint(worldPosition);
        }

        private UnityEngine.Camera _mainCamera;
    }
}
