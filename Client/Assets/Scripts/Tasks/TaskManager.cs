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
            if (_tasks.ContainsKey(task.Name))
                return false;

            // Add task:
            _tasks.Add(task.Name, task);

            // Raise event:
            if(OnTaskAdded != null)
                OnTaskAdded(this, new TaskEventArgs(task));

            return true;
        }

        public void RemoveTask(String taskName)
        {
            // Try to get task:
            Task task;
            if (!_tasks.TryGetValue(taskName, out task))
                return;

            // Remove it from the list:
            _tasks.Remove(taskName);

            // Raise event:
            if(OnTaskRemoved != null)
                OnTaskRemoved(this, new TaskEventArgs(task));
        }

        private readonly IDictionary<String, Task> _tasks = new Dictionary<String, Task>();
    }
}
