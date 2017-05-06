using System;
using System.Collections.Generic;
using CollaborationEngine.Network;

namespace CollaborationEngine.Tasks
{
    public class TaskManager
    {
        #region Classes
        public class TaskEventArgs : EventArgs
        {
            public TaskModel TaskModel { get; private set; }

            public TaskEventArgs(TaskModel taskModel)
            {
                TaskModel = taskModel;
            }
        }
        #endregion

        #region Delegates
        public delegate void TaskEventDelegate(TaskManager sender, TaskEventArgs eventArgs);
        #endregion

        #region Events
        public event TaskEventDelegate OnTaskAdded;
        public event TaskEventDelegate OnTaskRemoved;
        public event TaskEventDelegate OnTaskUpdated;
        #endregion

        #region Members
        private readonly List<TaskModel> _tasks = new List<TaskModel>();
        #endregion

        public bool AddTask(TaskModel taskModel)
        {
            // Return false if there is already a taskModel with the given name:
            if (_tasks.Exists(element => element.Name == taskModel.Name))
                return false;

            // Add taskModel:
            _tasks.Add(taskModel);

            // Subscribe to events:
            taskModel.OnNameChanged += Task_OnNameChanged;

            // Raise event:
            if (OnTaskAdded != null)
                OnTaskAdded(this, new TaskEventArgs(taskModel));

            return true;
        }
        public void RemoveTask(UInt32 taskID)
        {
            // Return if there is not any taskModel with the given ID:
            if (!_tasks.Exists(element => element.ID == taskID))
                return;

            // Find taskModel:
            var index = _tasks.FindIndex(element => element.ID == taskID);
            var task = _tasks[index];

            // Unsubscribe to events:
            task.OnNameChanged -= Task_OnNameChanged;

            // Remove it from the list:
            _tasks.RemoveAt(index);

            // Raise event:
            if (OnTaskRemoved != null)
                OnTaskRemoved(this, new TaskEventArgs(task));
        }
        public void UpdateTask(TaskModel taskModel)
        {
            // Return if there is not any taskModel with the given ID:
            if (!_tasks.Exists(element => element.ID == taskModel.ID))
                return;

            // Find taskModel:
            var taskToUpdate = _tasks.Find(element => element.ID == taskModel.ID);

            // Update taskModel:
            //taskToUpdate.Update(taskModel);
        }
        public TaskModel GetTask(UInt32 taskID)
        {
            if (!_tasks.Exists(element => element.ID == taskID))
                throw new Exception("There is not any taskModel with the given ID.");

            return _tasks.Find(element => element.ID == taskID);
        }
        public bool TryGetTask(UInt32 taskID, out TaskModel taskModel)
        {
            if (!_tasks.Exists(element => element.ID == taskID))
            {
                taskModel = null;
                return false;
            }

            taskModel = _tasks.Find(element => element.ID == taskID);

            return true;
        }

        #region Event Handlers
        private void Task_OnNameChanged(TaskModel sender, EventArgs eventArgs)
        {
            if (OnTaskUpdated != null)
                OnTaskUpdated(this, new TaskEventArgs(sender));
        }
        #endregion
    }
}
