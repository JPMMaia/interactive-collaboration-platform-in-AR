using CollaborationEngine.Cameras;
using UnityEngine;

namespace CollaborationEngine.Utilities
{
    public class CameraUtilities
    {
        public static Vector3 InFrontOfCameraPosition(ICamera camera)
        {
            var position = camera.UnityCamera.transform.position;
            switch (camera.CameraType)
            {
                case CameraViewType.Top:
                case CameraViewType.Bottom:
                    position.y = 0.0f;
                    break;

                case CameraViewType.Right:
                case CameraViewType.Left:
                    position.x = 0.0f;
                    break;

                case CameraViewType.Front:
                case CameraViewType.Back:
                    position.z = 0.0f;
                    break;

                default:
                    position = position + camera.UnityCamera.orthographicSize * camera.UnityCamera.transform.forward;
                    break;
            }

            return position;
        }

        public static Quaternion ParallelToCameraRotation(ICamera camera)
        {
            return Quaternion.FromToRotation(Vector3.forward, camera.UnityCamera.transform.forward);
        }
    }
}
