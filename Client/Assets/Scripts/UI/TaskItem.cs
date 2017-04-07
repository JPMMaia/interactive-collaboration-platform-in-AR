using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    public class TaskItem : MonoBehaviour
    {
        public delegate void TaskEventDelegate(TaskItem sender, EventArgs eventArgs);

        public event TaskEventDelegate OnClicked;
        public event TaskEventDelegate OnDeleted;

        public Text TaskNameText;

        private Task _task;

        public void Start()
        {
            OnEditClick();
        }
        public void OnDestroy()
        {
            if (Task != null)
                Task.OnNameChanged -= Task_OnNameChanged;
        }

        public void OnTaskClick()
        {
            if(OnClicked != null)
                OnClicked(this, EventArgs.Empty);
        }

        public void OnEditClick()
        {
            var editTaskPanel = Instantiate(ObjectLocator.Instance.EditTaskPanelPrefab);
            editTaskPanel.Task = Task;
            editTaskPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);

            editTaskPanel.TaskNameInputField.ActivateInputField();
        }

        public void OnDeleteClick()
        {
            if(OnDeleted != null)
                OnDeleted(this, EventArgs.Empty);

            Destroy(gameObject);
        }

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

        private void Task_OnNameChanged(Task sender, EventArgs eventArgs)
        {
            TaskNameText.text = _task.Name;
        }
    }
}
