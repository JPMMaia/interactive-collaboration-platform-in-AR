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

        public void AddTask(Task task)
        {
            _tasks.Add(task);

            if(OnTaskAdded != null)
                OnTaskAdded(this, new TaskEventArgs(task));
        }

        private List<Task> _tasks = new List<Task>();
    }
}
