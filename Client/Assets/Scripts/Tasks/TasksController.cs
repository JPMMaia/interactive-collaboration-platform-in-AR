using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using UnityEngine;

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
        private readonly List<TaskView> _taskViews = new List<TaskView>();
        #endregion

        public void Start()
        {
            foreach (var task in TasksModel.Tasks)
                CreateTaskView(task);
        }

        private void CreateTaskView(TaskModel taskModel)
        {
            // Instantiate:
            var taskView = Instantiate(TaskViewPrefab);

            // Set properties:
            taskView.TaskID = taskModel.ID;
            taskView.TaskName = taskModel.Name;

            // Subscribe to events:
            taskView.OnSelected += TaskView_OnSelected;
            taskView.OnEdited += TaskView_OnEdited;
            taskView.OnDuplicated += TaskView_OnDuplicated;
            taskView.OnDeleted += TaskView_OnDeleted;

            // Set parent:
            taskView.transform.SetParent(TasksView.Container, false);

            // Add to list:
            _taskViews.Add(taskView);
        }

        private void CreateTask()
        {
            // Create task model:
            var taskModel = TasksModel.Create();

            // Create task view:
            CreateTaskView(taskModel);
        }
        private void DeleteTask(uint taskID)
        {
            // Delete task model:
            TasksModel.Delete(taskID);

            // Find task view:
            var index = _taskViews.FindIndex(e => e.TaskID == taskID);
            if (index < 0)
                return;

            var taskView = _taskViews[index];

            // Remove task from list:
            _taskViews.RemoveAt(index);

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
        private void DuplicateTask(uint taskID)
        {

        }

        #region Event Handlers
        private void TaskView_OnSelected(object sender, Events.IDEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
        private void TaskView_OnEdited(object sender, Events.IDEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
        private void TaskView_OnDuplicated(object sender, Events.IDEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
        private void TaskView_OnDeleted(object sender, Events.IDEventArgs eventArgs)
        {
            DeleteTask(eventArgs.ID);
        }
        #endregion
    }
}
