using CollaborationEngine.Network;
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
                _task.OnStepAdded += Task_OnStepAdded;
                _task.OnStepDeleted += Task_OnStepDeleted;
                _task.OnStepUpdated += Task_OnStepUpdated;
            }

            {
                _stepPanel = Object.Instantiate(ObjectLocator.Instance.ServerStepsPanelPrefab);
                _stepPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
                _stepPanel.Task = _task;
                _stepPanel.OnStepItemClicked += StepPanel_OnStepItemClicked;
            }

            {

            }
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown StepState");

            if (_stepPanel)
            {
                _stepPanel.OnStepItemClicked += StepPanel_OnStepItemClicked;
                Object.Destroy(_stepPanel.gameObject);
                _stepPanel = null;
            }

            _task.OnStepUpdated -= Task_OnStepUpdated;
            _task.OnStepDeleted -= Task_OnStepDeleted;
            _task.OnStepAdded -= Task_OnStepAdded;
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }

        #region EventHandlers
        private void Task_OnStepAdded(Task sender, Task.StepEventArgs eventArgs)
        {

        }
        private void Task_OnStepDeleted(Task sender, Task.StepEventArgs eventArgs)
        {

        }
        private void Task_OnStepUpdated(Task sender, Task.StepEventArgs eventArgs)
        {

        }

        private void StepPanel_OnStepItemClicked(StepItem sender, System.EventArgs eventArgs)
        {
            // TODO next state
        }
        #endregion
    }
}
