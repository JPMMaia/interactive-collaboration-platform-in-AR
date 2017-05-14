namespace CollaborationEngine.Cameras
{
    public interface ICamera
    {
        UnityEngine.Camera UnityCamera { get; set; }
        bool Selected { get; set; }
    }
}
