using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Tasks
{
    public class TaskItem : MonoBehaviour
    {
        #region Delegates
        public delegate void TaskEventDelegate(TaskItem sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event TaskEventDelegate OnClicked;
        public event TaskEventDelegate OnDeleted;
        #endregion

        #region UnityEditor
        public Text TaskNameText;
        #endregion

        #region Properties
        public Task Task
        {
            get { return _task; }
            set
            {
                if (_task != null)
                    _task.OnNameChanged -= Task_OnNameChanged;

                _task = value;

                if (_task != null)
                    _task.OnNameChanged += Task_OnNameChanged;
            }
        }
        #endregion

        #region Members
        private Task _task;
        #endregion

        public void Start()
        {
            TaskNameText.text = _task.Name;
        }
        public void OnDestroy()
        {
            if (Task != null)
                Task.OnNameChanged -= Task_OnNameChanged;
        }

        #region Unity UI Events
        public void OnTaskClick()
        {
            if (OnClicked != null)
                OnClicked(this, EventArgs.Empty);
        }
        public void OnEditClick()
        {
            var editTaskPanel = Instantiate(ObjectLocator.Instance.EditTaskPanelPrefab);
            editTaskPanel.Task = Task;
        }
        public void OnDeleteClick()
        {
            if (OnDeleted != null)
                OnDeleted(this, EventArgs.Empty);

            Destroy(gameObject);
        }
        #endregion

        #region Event Handlers
        private void Task_OnNameChanged(Task sender, EventArgs eventArgs)
        {
            TaskNameText.text = _task.Name;
        }
        #endregion
    }
}
