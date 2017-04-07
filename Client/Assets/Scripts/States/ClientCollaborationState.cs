using CollaborationEngine.Objects;
using CollaborationEngine.Scenes;
using CollaborationEngine.States.Client;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.States
{
    public class ClientCollaborationState : IApplicationState
    {
        public void Initialize()
        {
            _currentState = new SelectTaskState(this);
            _currentState.Initialize();

            ObjectLocator.Instance.ClientRoot.SetActive(true);
            ObjectLocator.Instance.ServerRoot.SetActive(false);

            Scene = new Scene(ObjectLocator.Instance.SceneRoot);
        }
        public void Shutdown()
        {
            Scene = null;

            if (_currentState != null)
            {
                _currentState.Shutdown();
                _currentState = null;
            }
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
        public void FrameUpdate()
        {
            _currentState.FrameUpdate();
        }

        public Scene Scene { get; private set; }
        public TaskManager TaskManager
        {
            get
            {
                return _taskManager;
            }
        }
        public IApplicationState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState.Shutdown();
                _currentState = value;
                _currentState.Initialize();
            }
        }

        private IApplicationState _currentState;
        private readonly TaskManager _taskManager = new TaskManager();
    }
}
