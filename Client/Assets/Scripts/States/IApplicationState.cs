namespace CollaborationEngine.States
{
    public interface IApplicationState
    {
        void Initialize();
        void Shutdown();
        void FixedUpdate();
        void FrameUpdate();
    }
}
