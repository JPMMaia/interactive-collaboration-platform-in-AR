using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using JetBrains.Annotations;

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
                CreateTaskView(task);
        }
        public void OnDisable()
        {
            // Destroy task views:
            foreach (var taskView in _taskViews)
                Destroy(taskView.Value.gameObject);
            _taskViews.Clear();

            // Unsubscribe to events:
            TasksView.OnCreateTaskClicked -= TasksView_OnCreateTaskClicked;
            TasksModel.OnTaskDeleted -= TasksModel_OnTaskDeleted;
            TasksModel.OnTaskDuplicated -= TasksModel_OnTaskDuplicated;
            TasksModel.OnTaskCreated -= TasksModel_OnTaskCreated;
        }

        private TaskView CreateTaskView([NotNull] TaskModel taskModel)
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

            // Set parent:
            taskView.transform.SetParent(TasksView.Container, false);

            // Add to list:
            _taskViews.Add(taskModel.ID, taskView);

            return taskView;
        }
        private void DeleteTaskView(uint taskID)
        {
            // Get task view:
            TaskView taskView;
            if (!_taskViews.TryGetValue(taskID, out taskView))
                return;

            // Remove task from list:
            _taskViews.Remove(taskID);

            // Remove from parent:
            taskView.transform.SetParent(null);

            // Unsubscribe from events:
            taskView.OnDeleted += TaskView_OnDeleted;
            taskView.OnDuplicated += TaskView_OnDuplicated;
            taskView.OnEdited += TaskView_OnEdited;
            taskView.OnSelected += TaskView_OnSelected;

            // Destroy:
            Destroy(taskView.gameObject);
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
            // Create task model:
            var taskModel = TasksModel.Create();

            // Get corresponding task view:
            TaskView taskView;
            if (!_taskViews.TryGetValue(taskModel.ID, out taskView))
                taskView = CreateTaskView(taskModel);

            // Edit task name:
            taskView.EditTaskName();
        }

        private void TaskView_OnSelected(TaskView sender, Events.IDEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
        private void TaskView_OnEdited(TaskView sender, Events.IDEventArgs eventArgs)
        {
            sender.EditTaskName();
        }
        private void TaskView_OnDuplicated(TaskView sender, Events.IDEventArgs eventArgs)
        {
            TasksModel.Duplicate(eventArgs.ID);
        }
        private void TaskView_OnDeleted(TaskView sender, Events.IDEventArgs eventArgs)
        {
            TasksModel.Delete(eventArgs.ID);
        }
        #endregion
    }
}
