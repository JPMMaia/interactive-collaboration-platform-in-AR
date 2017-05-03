using System;
using System.Collections.Generic;

namespace CollaborationEngine.Tasks
{
    public class TaskManager
    {
        #region Classes
        public class TaskEventArgs : EventArgs
        {
            public Task Task { get; private set; }

            public TaskEventArgs(Task task)
            {
                Task = task;
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
        private readonly List<Task> _tasks = new List<Task>();
        #endregion

        public bool AddTask(Task task)
        {
            // Return false if there is already a task with the given name:
            if (_tasks.Exists(element => element.Name == task.Name))
                return false;

            // Add task:
            _tasks.Add(task);

            // Subscribe to events:
            task.OnNameChanged += Task_OnNameChanged;

            // Raise event:
            if (OnTaskAdded != null)
                OnTaskAdded(this, new TaskEventArgs(task));

            return true;
        }
        public void RemoveTask(UInt32 taskID)
        {
            // Return if there is not any task with the given ID:
            if (!_tasks.Exists(element => element.ID == taskID))
                return;

            // Find task:
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
        public void UpdateTask(Task task)
        {
            // Return if there is not any task with the given ID:
            if (!_tasks.Exists(element => element.ID == task.ID))
                return;

            // Find task:
            var taskToUpdate = _tasks.Find(element => element.ID == task.ID);

            // Update task:
            taskToUpdate.Update(task);
        }
        public Task GetTask(UInt32 taskID)
        {
            if (!_tasks.Exists(element => element.ID == taskID))
                throw new Exception("There is not any task with the given ID.");

            return _tasks.Find(element => element.ID == taskID);
        }
        public bool TryGetTask(UInt32 taskID, out Task task)
        {
            if (!_tasks.Exists(element => element.ID == taskID))
            {
                task = null;
                return false;
            }

            task = _tasks.Find(element => element.ID == taskID);

            return true;
        }

        #region Event Handlers
        private void Task_OnNameChanged(Task sender, EventArgs eventArgs)
        {
            if (OnTaskUpdated != null)
                OnTaskUpdated(this, new TaskEventArgs(sender));
        }
        #endregion
    }
}
