using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class TasksController : Controller
    {
        #region Unity Editor
        public TaskView TaskViewPrefab;
        public TasksView TasksView;
        #endregion

        #region Properties
        private TasksModel TasksModel
        {
            get { return Application.Model.Tasks; }
        }
        #endregion

        #region Members
        private readonly Dictionary<uint, TaskView> _taskViews = new Dictionary<uint, TaskView>();
        #endregion

        public void OnEnable()
        {
            // Subscribe to events:
            TasksModel.OnTaskCreated += TasksModel_OnTaskCreated;
            TasksModel.OnTaskDuplicated += TasksModel_OnTaskDuplicated;
            TasksModel.OnTaskDeleted += TasksModel_OnTaskDeleted;
            TasksView.OnCreateTaskClicked += TasksView_OnCreateTaskClicked;

            // Create task views:
            foreach (var task in TasksModel.Tasks)
                CreateTaskView(task.Value);
        }

        private TaskView CreateTaskView(TaskModel taskModel)
        {
            // Ignore if task view already exists:
            if (_taskViews.ContainsKey(taskModel.ID))
                return _taskViews[taskModel.ID];

            // Instantiate:
            var taskView = Instantiate(TaskViewPrefab);

            // Set properties:
            taskView.TaskID = taskModel.ID;
            taskView.TaskOrder = (uint)_taskViews.Count + 1;
            taskView.TaskName = taskModel.Name;

            // Subscribe to events:
            taskView.OnSelected += TaskView_OnSelected;
            taskView.OnEdited += TaskView_OnEdited;
            taskView.OnDuplicated += TaskView_OnDuplicated;
            taskView.OnDeleted += TaskView_OnDeleted;
            taskView.OnEndEdit += TaskView_OnEndEdit;

            // Add to task view:
            TasksView.AddToContainer(taskView.transform);

            // Add to list:
            _taskViews.Add(taskModel.ID, taskView);

            return taskView;
        }
        private void DeleteTaskView(uint taskID)
        {
            // GetStep task view:
            TaskView taskView;
            if (!_taskViews.TryGetValue(taskID, out taskView))
                return;

            // Remove task from list:
            _taskViews.Remove(taskID);

            // Remove from task view:
            TasksView.RemoveFromContainer(taskView.transform);

            // Unsubscribe from events:
            taskView.OnEndEdit -= TaskView_OnEndEdit;
            taskView.OnDeleted -= TaskView_OnDeleted;
            taskView.OnDuplicated -= TaskView_OnDuplicated;
            taskView.OnEdited -= TaskView_OnEdited;
            taskView.OnSelected -= TaskView_OnSelected;

            // Destroy:
            Destroy(taskView.gameObject);
        }

        private void ActivateTaskViewInputField(TaskView taskView)
        {
            // Block UI:
            TasksView.Interactale = false;

            // Edit task name:
            taskView.EditTaskName();
        }

        #region Event Handlers
        private void TasksModel_OnTaskCreated(TasksModel sender, TaskEventArgs eventArgs)
        {
            CreateTaskView(eventArgs.TaskModel);
        }
        private void TasksModel_OnTaskDuplicated(TasksModel sender, TaskEventArgs eventArgs)
        {
            CreateTaskView(eventArgs.TaskModel);
        }
        private void TasksModel_OnTaskDeleted(TasksModel sender, TaskEventArgs eventArgs)
        {
            DeleteTaskView(eventArgs.TaskModel.ID);
        }

        private void TasksView_OnCreateTaskClicked(object sender, EventArgs eventArgs)
        {
            // CreateStep task model:
            var taskModel = TasksModel.CreateTask();

            // GetStep corresponding task view:
            TaskView taskView;
            if (!_taskViews.TryGetValue(taskModel.ID, out taskView))
                taskView = CreateTaskView(taskModel);

            ActivateTaskViewInputField(taskView);
        }

        private void TaskView_OnSelected(TaskView sender, Events.IDEventArgs eventArgs)
        {
            TasksView.NotifyTaskSelected(eventArgs.ID);
        }
        private void TaskView_OnEdited(TaskView sender, Events.IDEventArgs eventArgs)
        {
            ActivateTaskViewInputField(sender);
        }
        private void TaskView_OnDuplicated(TaskView sender, Events.IDEventArgs eventArgs)
        {
            TasksModel.DuplicateTask(eventArgs.ID);
        }
        private void TaskView_OnDeleted(TaskView sender, Events.IDEventArgs eventArgs)
        {
            TasksModel.DeleteTask(eventArgs.ID);
        }
        private void TaskView_OnEndEdit(TaskView sender, Events.IDEventArgs eventArgs)
        {
            // GetStep task model:
            var taskModel = TasksModel.GetTask(eventArgs.ID);

            // Update name:
            taskModel.Name = sender.TaskName;

            // Unblock UI:
            TasksView.Interactale = true;
        }
        #endregion
    }
}
