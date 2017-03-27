using CollaborationEngine.Objects;
using CollaborationEngine.Scenes;

namespace CollaborationEngine.States
{
    public class ClientCollaborationState : IApplicationState
    {
        public Scene Scene { get; private set; }

        public void Initialize()
        {
            ObjectLocator.Instance.ClientRoot.SetActive(true);
            ObjectLocator.Instance.ServerRoot.SetActive(false);

            Scene = new Scene(ObjectLocator.Instance.SceneRoot);
        }

        public void Shutdown()
        {
            Scene = null;
        }

        public void FixedUpdate()
        {
        }

        public void FrameUpdate()
        {
        }
    }
}
