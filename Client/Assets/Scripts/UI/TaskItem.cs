using System;
using CollaborationEngine.Objects;
using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    public class TaskItem : MonoBehaviour
    {
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
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                var serverState = currentState as ServerCollaborationState;
                //serverState.CurrentState = new StepState(serverState, );
                // TODO
            }
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
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                var serverState = currentState as ServerCollaborationState;
                serverState.TaskManager.RemoveTask(Task.Name);
                Task = null;
            }

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
