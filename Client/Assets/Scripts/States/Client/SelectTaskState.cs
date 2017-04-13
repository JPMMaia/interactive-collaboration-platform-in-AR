﻿using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI.Tasks;
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
            networkManager.RegisterHandler(NetworkHandles.AddTaskHandle, OnAddTask);
            networkManager.RegisterHandler(NetworkHandles.RemoveTaskHandle, OnRemoveTask);
            networkManager.RegisterHandler(NetworkHandles.UpdateTaskHandle, OnUpdateTask);

            _taskPanel = Object.Instantiate(ObjectLocator.Instance.ClientTaskPanelPrefab);
            _taskPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
            _taskPanel.TaskManager = _clientState.TaskManager;
            _taskPanel.OnTaskItemClicked += TaskPanel_OnTaskItemClicked;
        }
        public void Shutdown()
        {
            if (_taskPanel != null)
            {
                _taskPanel.OnTaskItemClicked -= TaskPanel_OnTaskItemClicked;
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

        private void TaskPanel_OnTaskItemClicked(TaskItem sender, System.EventArgs eventArgs)
        {
            _clientState.CurrentState = new StepState(_clientState, sender.Task);
        }

        private readonly ClientCollaborationState _clientState;
        private TasksPanel _taskPanel;
    }
}