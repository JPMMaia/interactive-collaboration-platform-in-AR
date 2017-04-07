using System;
using System.Collections.Generic;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class TasksPanel : MonoBehaviour
    {
        public event TaskItem.TaskEventDelegate OnTaskItemClicked;

        public TaskItem TaskItemPrefab;
        public RectTransform Content;

        public void Start()
        {
            var taskButtonTransform = TaskItemPrefab.GetComponent<RectTransform>();
            _taskButtonHeight = taskButtonTransform.rect.size.y;

            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);

            TaskManager.OnTaskAdded += TaskManager_OnTaskAdded;
            TaskManager.OnTaskRemoved += TaskManager_OnTaskRemoved;
        }
        public void OnDestroy()
        {
            if (TaskManager != null)
            {
                TaskManager.OnTaskRemoved -= TaskManager_OnTaskRemoved;
                TaskManager.OnTaskAdded -= TaskManager_OnTaskAdded;
                TaskManager = null;
            }
        }

        public TaskManager TaskManager { get; set; }

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
            taskItem.OnClicked += TaskItem_OnClicked;
            taskItem.OnDeleted += TaskItem_OnDeleted;

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
            taskItem.OnDeleted -= TaskItem_OnDeleted;
            taskItem.OnClicked -= TaskItem_OnClicked;
            taskItem.transform.SetParent(null);
            Destroy(taskItem);

            // Deallocate space of deleted element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _taskItems.Count * _taskButtonHeight);
        }

        private void TaskItem_OnClicked(TaskItem sender, EventArgs eventArgs)
        {
            if(OnTaskItemClicked != null)
                OnTaskItemClicked(sender, EventArgs.Empty);
        }
        private void TaskItem_OnDeleted(TaskItem sender, EventArgs eventArgs)
        {
            TaskManager.RemoveTask(sender.Task.Name);
        }
    }
}
