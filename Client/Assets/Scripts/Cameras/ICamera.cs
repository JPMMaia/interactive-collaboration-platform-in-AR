using JetBrains.Annotations;

namespace CollaborationEngine.Cameras
{
    public interface ICamera
    {
        CameraViewType CameraType { get; }
        UnityEngine.Camera UnityCamera { get; }
        bool Selected { get; set; }
    }
}
