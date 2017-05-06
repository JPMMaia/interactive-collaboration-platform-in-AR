using System;
using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI.Tasks
{
    public class TasksPanel : MonoBehaviour
    {
        #region Events
        public event TaskItem.TaskEventDelegate OnTaskItemClicked;
        #endregion

        #region Unity Editor
        public TaskItem TaskItemPrefab;
        public RectTransform Content;
        #endregion

        #region Properties
        public TaskManager TaskManager { get; set; }
        #endregion

        #region Members
        private readonly List<TaskItem> _taskItems = new List<TaskItem>();
        private float _taskButtonHeight;
        #endregion

        public void Start()
        {
            var taskButtonTransform = TaskItemPrefab.GetComponent<RectTransform>();
            _taskButtonHeight = taskButtonTransform.rect.size.y;

            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);

            TaskManager.OnTaskAdded += TaskManager_OnTaskAdded;
            TaskManager.OnTaskRemoved += TaskManager_OnTaskRemoved;

            ObjectLocator.Instance.HintText.SetText("Create, delete or select a taskModel.");
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

        #region Unity UI
        public void OnCreateNewTaskButtonClicked()
        {
            Instantiate(ObjectLocator.Instance.EditTaskPanelPrefab);
        }
        #endregion

        #region Event Handlers
        private void TaskManager_OnTaskAdded(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            // Allocate space for new element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_taskItems.Count + 1) * _taskButtonHeight);

            // Add new element:
            var position = new Vector3(0.0f, -_taskItems.Count * _taskButtonHeight);
            var taskItem = Instantiate(TaskItemPrefab, position, Quaternion.identity);
            taskItem.TaskModel = eventArgs.TaskModel;
            taskItem.transform.SetParent(Content.transform, false);
            taskItem.OnClicked += TaskItem_OnClicked;
            taskItem.OnDeleted += TaskItem_OnDeleted;

            // Add to list:
            _taskItems.Add(taskItem);
        }
        private void TaskManager_OnTaskRemoved(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            // Return if taskModel item does not exist:
            if (!_taskItems.Exists(element => element.TaskModel.ID == eventArgs.TaskModel.ID))
                return;

            // Find element:
            var index = _taskItems.FindIndex(element => element.TaskModel.ID == eventArgs.TaskModel.ID);
            var taskItem = _taskItems[index];

            // Remove from list:
            _taskItems.RemoveAt(index);

            // Destroy taskModel item:
            taskItem.OnDeleted -= TaskItem_OnDeleted;
            taskItem.OnClicked -= TaskItem_OnClicked;
            taskItem.transform.SetParent(null);
            Destroy(taskItem);

            // Deallocate space of deleted element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _taskItems.Count * _taskButtonHeight);
        }

        private void TaskItem_OnClicked(TaskItem sender, EventArgs eventArgs)
        {
            if (OnTaskItemClicked != null)
                OnTaskItemClicked(sender, EventArgs.Empty);
        }
        private void TaskItem_OnDeleted(TaskItem sender, EventArgs eventArgs)
        {
            TaskManager.RemoveTask(sender.TaskModel.ID);
        }
        #endregion
    }
}
