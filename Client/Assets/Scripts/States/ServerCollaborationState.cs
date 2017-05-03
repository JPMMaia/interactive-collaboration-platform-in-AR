using CollaborationEngine.Objects;
using CollaborationEngine.States.Server;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.States
{
    public class ServerCollaborationState : IApplicationState
    {
        #region Members
        private IApplicationState _currentState;
        private InstructionType _selectedInstructionType = InstructionType.Arrow;
        private readonly TaskManager _taskManager = new TaskManager();
        #endregion

        public void Initialize()
        {
            _currentState = new SelectTaskState(this);
            _currentState.Initialize();

            ObjectLocator.Instance.ServerRoot.SetActive(true);
            ObjectLocator.Instance.ClientRoot.SetActive(false);
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
            _currentState.FixedUpdate();
        }
        public void FrameUpdate()
        {
            _currentState.FrameUpdate();
        }

        public InstructionType SelectedInstructionType
        {
            get
            {
                return _selectedInstructionType;
            }
            set { _selectedInstructionType = value; }
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
    }
}
