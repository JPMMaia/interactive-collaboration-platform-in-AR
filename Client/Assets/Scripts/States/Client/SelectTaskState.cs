using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

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
            var networkManager = NetworkManager.singleton.client;
            networkManager.RegisterHandler(NetworkHandles.AddTaskOnClientHandle, OnAddTask);
            networkManager.RegisterHandler(NetworkHandles.RemoveTaskOnClientHandle, OnRemoveTask);
            networkManager.RegisterHandler(NetworkHandles.UpdateTaskOnClientHandle, OnUpdateTask);

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

        private void OnAddTask(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<Task.TaskMesssage>();
            _clientState.TaskManager.AddTask(message.Data);
        }
        private void OnRemoveTask(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<Task.TaskMesssage>();
            _clientState.TaskManager.RemoveTask(message.Data.ID);
        }
        private void OnUpdateTask(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<Task.TaskMesssage>();
            _clientState.TaskManager.UpdateTask(message.Data);
        }

        private readonly ClientCollaborationState _clientState;
        private TasksPanel _taskPanel;
    }
}
