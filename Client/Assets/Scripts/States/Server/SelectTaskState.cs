using CollaborationEngine.Objects;
using CollaborationEngine.UI.Tasks;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class SelectTaskState : IApplicationState
    {
        #region Members
        private readonly ServerCollaborationState _serverState;
        private TasksPanel _taskPanel;
        #endregion

        public SelectTaskState(ServerCollaborationState serverState)
        {
            _serverState = serverState;
        }

        public void Initialize()
        {
            Debug.Log("Initialize SelectTaskState");

            _taskPanel = Object.Instantiate(ObjectLocator.Instance.ServerTaskPanelPrefab);
            _taskPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
            _taskPanel.TaskManager = _serverState.TaskManager;
            _taskPanel.OnTaskItemClicked += TaskPanel_OnTaskItemClicked;
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown SelectTaskState");

            if (_taskPanel != null)
            {
                Object.Destroy(_taskPanel.gameObject);
                _taskPanel = null;
            }
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }
        public void LateUpdate()
        {
        }

        private void TaskPanel_OnTaskItemClicked(TaskItem sender, System.EventArgs eventArgs)
        {
            _serverState.CurrentState = new StepState(_serverState, sender.Task);
        }
    }
}
