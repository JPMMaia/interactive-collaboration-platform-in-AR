using System;

namespace CollaborationEngine.Tasks
{
    public class TaskEventArgs : EventArgs
    {
        public TaskModel TaskModel { get; private set; }

        public TaskEventArgs(TaskModel taskModel)
        {
            TaskModel = taskModel;
        }
    }
}
