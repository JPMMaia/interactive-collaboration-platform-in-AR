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
        public IEnumerable<KeyValuePair<uint, TaskModel>> Tasks
        {
            get { return _tasks; }
        }
        #endregion

        private readonly Dictionary<uint, TaskModel> _tasks = new Dictionary<uint, TaskModel>();

        private TaskModel CreateTask()
        {
            // Create new task and assign a unique ID:
            var task = Instantiate(TaskModelPrefab);
            task.ID = TaskModel.GenerateID();

            // Add task to list:
            _tasks.Add(task.ID, task);

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
            var taskToDuplicate = _tasks[taskID];

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
            TaskModel task;
            if(!_tasks.TryGetValue(taskID, out task))
                return;

            // Remove task:
            _tasks.Remove(taskID);

            // Raise event:
            if (OnTaskDeleted != null)
                OnTaskDeleted(this, new TaskEventArgs(task));
        }
        public TaskModel Get(uint taskID)
        {
            return _tasks[taskID];
        }
    }
}
