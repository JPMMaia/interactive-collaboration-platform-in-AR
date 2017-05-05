using CollaborationEngine.Objects;
using CollaborationEngine.States.Client;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.States
{
    public class ClientCollaborationState : IApplicationState
    {
        public void Initialize()
        {
            _currentState = new WaitForStepState(this);
            _currentState.Initialize();

            ObjectLocator.Instance.ClientRoot.SetActive(true);
            ObjectLocator.Instance.ServerRoot.SetActive(false);
        }
        public void Shutdown()
        {
            if (_currentState != null)
            {
                _currentState.Shutdown();
                _currentState = null;
            }
        }

        public void FixedUpdate()
        {
            if (_currentState != null)
                _currentState.FixedUpdate();
        }
        public void FrameUpdate()
        {
            if (_currentState != null)
                _currentState.FrameUpdate();
        }
        public void LateUpdate()
        {
            if (_currentState != null)
                _currentState.LateUpdate();
        }

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
