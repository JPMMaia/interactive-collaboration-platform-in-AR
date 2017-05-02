using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI.Steps;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class StepState : IApplicationState
    {
        #region Members
        private readonly ServerCollaborationState _serverState;
        private readonly Task _task;
        private StepsPanel _stepPanel;
        #endregion

        public StepState(ServerCollaborationState serverState, Task task)
        {
            _serverState = serverState;
            _task = task;
        }

        public void Initialize()
        {
            Debug.Log("Initialize StepState");

            {
                _stepPanel = Object.Instantiate(ObjectLocator.Instance.ServerStepsPanelPrefab);
                _stepPanel.Task = _task;
                ObjectLocator.Instance.LeftPanel.Add(_stepPanel.GetComponent<RectTransform>());
            }

            {
                var consolePanel = Object.Instantiate(ObjectLocator.Instance.ConsoleController);
                ObjectLocator.Instance.ConsoleController = consolePanel;
                ObjectLocator.Instance.LeftPanel.Add(consolePanel.GetComponent<RectTransform>());
            }
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown StepState");

            if (_stepPanel)
            {
                Object.Destroy(_stepPanel.gameObject);
                _stepPanel = null;
            }
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }
    }
}
