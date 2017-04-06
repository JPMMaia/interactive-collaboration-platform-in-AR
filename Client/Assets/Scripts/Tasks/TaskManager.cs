using System;
using System.Collections.Generic;

namespace CollaborationEngine.Tasks
{
    public class TaskManager
    {
        public class TaskEventArgs : EventArgs
        {
            public Task Task { get; private set; }

            public TaskEventArgs(Task task)
            {
                Task = task;
            }
        }

        public delegate void TaskEventDelegate(TaskManager sender, TaskEventArgs eventArgs);

        public event TaskEventDelegate OnTaskAdded;
        public event TaskEventDelegate OnTaskRemoved;

        public bool AddTask(Task task)
        {
            // Return false if there is already a task with the given name:
            if(_tasks.Exists(element => element.Name == task.Name))
                return false;

            // Add task:
            _tasks.Add(task);

            // Raise event:
            if(OnTaskAdded != null)
                OnTaskAdded(this, new TaskEventArgs(task));

            return true;
        }
        public void RemoveTask(String taskName)
        {
            // Return if there is not any task with the given name:
            if (!_tasks.Exists(element => element.Name == taskName))
                return;

            // Find task:
            var index = _tasks.FindIndex(element => element.Name == taskName);
            var task = _tasks[index];

            // Remove it from the list:
            _tasks.RemoveAt(index);

            // Raise event:
            if(OnTaskRemoved != null)
                OnTaskRemoved(this, new TaskEventArgs(task));
        }
        public Task GetTask(String taskName)
        {
            if(!_tasks.Exists(element => element.Name == taskName))
                throw new Exception("There is not any task with the given name.");

            return _tasks.Find(element => element.Name == taskName);
        }
        public bool TryGetTask(String taskName, out Task task)
        {
            if (!_tasks.Exists(element => element.Name == taskName))
            {
                task = null;
                return false;
            }

            task = _tasks.Find(element => element.Name == taskName);

            return true;
        }

        private readonly List<Task> _tasks = new List<Task>();
    }
}
