using System.Collections.Generic;
using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class TasksPanel : MonoBehaviour
    {
        public TaskItem TaskItemPrefab;
        public RectTransform Content;

        public void Start()
        {
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                _serverState = currentState as ServerCollaborationState;
                _serverState.TaskManager.OnTaskAdded += TaskManager_OnTaskAdded;
                _serverState.TaskManager.OnTaskRemoved += TaskManager_OnTaskRemoved;
            }

            var taskButtonTransform = TaskItemPrefab.GetComponent<RectTransform>();
            _taskButtonHeight = taskButtonTransform.rect.size.y;

            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);
        }
        public void OnApplicationQuit()
        {
            if (_serverState != null)
            {
                _serverState.TaskManager.OnTaskRemoved -= TaskManager_OnTaskRemoved;
                _serverState.TaskManager.OnTaskAdded -= TaskManager_OnTaskAdded;
                _serverState = null;
            }
        }

        private ServerCollaborationState _serverState;
        private readonly List<TaskItem> _taskItems = new List<TaskItem>();
        private float _taskButtonHeight;

        private void TaskManager_OnTaskAdded(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            // Allocate space for new element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_taskItems.Count + 1) * _taskButtonHeight);

            // Add new element:
            var position = new Vector3(0.0f, -_taskItems.Count * _taskButtonHeight);
            var taskItem = Instantiate(TaskItemPrefab, position, Quaternion.identity);
            taskItem.Task = eventArgs.Task;
            taskItem.transform.SetParent(Content.transform, false);

            // Add to list:
            _taskItems.Add(taskItem);
        }
        private void TaskManager_OnTaskRemoved(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            // Return if task item does not exist:
            if (!_taskItems.Exists(element => element.Task.Name == eventArgs.Task.Name))
                return;

            // Find element:
            var index = _taskItems.FindIndex(element => element.Task.Name == eventArgs.Task.Name);
            var taskItem = _taskItems[index];

            // Remove from list:
            _taskItems.RemoveAt(index);

            // Destroy task item:
            taskItem.transform.SetParent(null);
            Destroy(taskItem);

            // Deallocate space of deleted element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _taskItems.Count * _taskButtonHeight);
        }
    }
}
