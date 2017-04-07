using CollaborationEngine.Objects;
using CollaborationEngine.UI;
using UnityEngine;

namespace CollaborationEngine.States.Client
{
    public class SelectTaskState : IApplicationState
    {
        public SelectTaskState(ClientCollaborationState clientState)
        {
            _clientState = clientState;
        }

        public void Initialize()
        {
            _taskPanel = Object.Instantiate(ObjectLocator.Instance.ClientTaskPanelPrefab);
            _taskPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
            _taskPanel.TaskManager = _clientState.TaskManager;
        }
        public void Shutdown()
        {
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

        private readonly ClientCollaborationState _clientState;
        private TasksPanel _taskPanel;
    }
}
