namespace CollaborationEngine.Objects.Components
{
    public interface IComponent
    {
        void Instantiate();
        void Destroy();
        void Update();
    }
}
