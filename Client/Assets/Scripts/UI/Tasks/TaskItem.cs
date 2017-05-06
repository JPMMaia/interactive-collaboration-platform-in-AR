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
        public TaskModel TaskModel
        {
            get { return _taskModel; }
            set
            {
                if (_taskModel != null)
                    _taskModel.OnNameChanged -= TaskModelOnNameChanged;

                _taskModel = value;

                if (_taskModel != null)
                    _taskModel.OnNameChanged += TaskModelOnNameChanged;
            }
        }
        #endregion

        #region Members
        private TaskModel _taskModel;
        #endregion

        public void Start()
        {
            TaskNameText.text = _taskModel.Name;
        }
        public void OnDestroy()
        {
            if (TaskModel != null)
                TaskModel.OnNameChanged -= TaskModelOnNameChanged;
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
            editTaskPanel.TaskModel = TaskModel;
        }
        public void OnDeleteClick()
        {
            if (OnDeleted != null)
                OnDeleted(this, EventArgs.Empty);

            Destroy(gameObject);
        }
        #endregion

        #region Event Handlers
        private void TaskModelOnNameChanged(TaskModel sender, EventArgs eventArgs)
        {
            TaskNameText.text = _taskModel.Name;
        }
        #endregion
    }
}
