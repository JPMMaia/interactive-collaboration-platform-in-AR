using System;
using CollaborationEngine.Base;
using UnityEngine.UI;

namespace CollaborationEngine.Tasks
{
    public class TaskView : Entity
    {
        #region UnityEditor
        public Text TaskNameText;
        #endregion

        #region Properties
        public TaskModel Task { get; set; }
        #endregion

        public void Awake()
        {
            TaskNameText.text = Task.Name;
        }

        #region Unity UI Events
        public void OnTaskClick()
        {
            Application.Notify(TaskNotification.Selected.ToString(), this, Task);
        }
        public void OnEditClick()
        {
            Application.Notify(TaskNotification.Edited.ToString(), this, Task);
        }
        public void OnDeleteClick()
        {
            Application.Notify(TaskNotification.Deleted.ToString(), this, Task);
        }
        #endregion
    }
}
