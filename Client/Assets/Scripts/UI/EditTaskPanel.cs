using CollaborationEngine.Objects;
using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    public class EditTaskPanel : MonoBehaviour
    {
        public void Start()
        {
            transform.SetParent(ObjectLocator.Instance.UICanvas, false);

            if (Task != null)
                TaskNameInputField.text = Task.Name;

            TaskNameInputField.ActivateInputField();
        }

        public void OnOKClick()
        {
            if (Task != null)
            {
                Task.Name = TaskNameInputField.text;
            }
            else
            {
                var currentState = ApplicationInstance.Instance.CurrentState;
                if (currentState is ServerCollaborationState)
                {
                    var serverState = currentState as ServerCollaborationState;
                    serverState.TaskManager.AddTask(new Task(TaskNameInputField.text));
                }
            }

            Destroy();
        }

        public Task Task { get; set; }

        public InputField TaskNameInputField;

        private void Destroy()
        {
            Task = null;

            Destroy(gameObject);
        }
    }
}
