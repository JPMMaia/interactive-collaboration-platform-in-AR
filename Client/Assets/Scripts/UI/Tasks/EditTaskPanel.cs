using CollaborationEngine.Objects;
using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Tasks
{
    public class EditTaskPanel : MonoBehaviour
    {
        #region Unity Editor
        public InputField TaskNameInputField;
        #endregion

        #region Properties
        public Task Task { get; set; }
        #endregion

        public void Start()
        {
            transform.SetParent(ObjectLocator.Instance.UICanvas, false);

            if (Task != null)
                TaskNameInputField.text = Task.Name;

            TaskNameInputField.ActivateInputField();
        }

        #region Unity UI Events
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

            Destroy(gameObject);
        }
        #endregion
    }
}
