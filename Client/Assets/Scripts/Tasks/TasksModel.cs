using System.Collections.Generic;
using CollaborationEngine.Base;

namespace CollaborationEngine.Tasks
{
    public class TasksModel : Entity
    {
        #region Events
        public delegate void TaskModelEventDelegate(TasksModel sender, TaskEventArgs eventArgs);

        public event TaskModelEventDelegate OnTaskCreated;
        public event TaskModelEventDelegate OnTaskDuplicated;
        public event TaskModelEventDelegate OnTaskDeleted;
        #endregion

        #region Unity Editor
        public TaskModel TaskModelPrefab;
        #endregion

        #region Properties
        public IEnumerable<TaskModel> Tasks
        {
            get { return _tasks; }
        }
        #endregion

        private readonly List<TaskModel> _tasks = new List<TaskModel>();

        private TaskModel CreateTask()
        {
            // Create new task and assign a unique ID:
            var task = Instantiate(TaskModelPrefab);
            task.ID = TaskModel.GenerateID();

            // Add task to list:
            _tasks.Add(task);

            return task;
        }
        public TaskModel Create()
        {
            var task = CreateTask();

            // Raise event:
            if(OnTaskCreated != null)
                OnTaskCreated(this, new TaskEventArgs(task));

            return task;
        }
        public TaskModel Duplicate(uint taskID)
        {
            // Get task to duplicate:
            var taskToDuplicate = Get(taskID);

            // Create new task and perform deep copy:
            var duplicatedTask = CreateTask();
            taskToDuplicate.DeepCopy(duplicatedTask);

            // Raise event:
            if (OnTaskDuplicated != null)
                OnTaskDuplicated(this, new TaskEventArgs(duplicatedTask));

            return duplicatedTask;
        }
        public void Delete(uint taskID)
        {
            // Get task:
            var index = _tasks.FindIndex(e => e.ID == taskID);
            if (index < 0)
                return;

            var task = _tasks[index];

            // Remove task:
            _tasks.RemoveAt(index);

            // Raise event:
            if (OnTaskDeleted != null)
                OnTaskDeleted(this, new TaskEventArgs(task));
        }
        public TaskModel Get(uint taskID)
        {
            return _tasks.Find(e => e.ID == taskID);
        }
    }
}
