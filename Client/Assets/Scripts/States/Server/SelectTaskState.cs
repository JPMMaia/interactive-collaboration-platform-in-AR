using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.States.Server
{
    public class SelectTaskState : IApplicationState
    {
        public SelectTaskState(ServerCollaborationState serverState)
        {
            _serverState = serverState;
        }

        public void Initialize()
        {
            Debug.Log("Initialize SelectTaskState");

            _serverState.TaskManager.OnTaskAdded += TaskManager_OnTaskAdded;
            _serverState.TaskManager.OnTaskRemoved += TaskManager_OnTaskRemoved;
            _serverState.TaskManager.OnTaskUpdated += TaskManager_OnTaskUpdated;

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

            _serverState.TaskManager.OnTaskUpdated -= TaskManager_OnTaskUpdated;
            _serverState.TaskManager.OnTaskRemoved -= TaskManager_OnTaskRemoved;
            _serverState.TaskManager.OnTaskAdded -= TaskManager_OnTaskAdded;
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }

        private void TaskManager_OnTaskAdded(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.AddTaskOnServerHandle, new Task.TaskMesssage { Data = eventArgs.Task });
        }
        private void TaskManager_OnTaskRemoved(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.RemoveTaskOnServerHandle, new Task.TaskMesssage { Data = eventArgs.Task });
        }
        private void TaskManager_OnTaskUpdated(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.UpdateTaskOnServerHandle, new Task.TaskMesssage { Data = eventArgs.Task });
        }

        private void TaskPanel_OnTaskItemClicked(TaskItem sender, System.EventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }

        private readonly ServerCollaborationState _serverState;
        private TasksPanel _taskPanel;
    }
}
