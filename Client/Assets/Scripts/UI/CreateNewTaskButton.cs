using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class CreateNewTaskButton : MonoBehaviour
    {
        public void OnClicked()
        {
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                var serverState = currentState as ServerCollaborationState;
                serverState.TaskManager.AddTask(new Task("Task 123"));

                // TODO input text from user
            }
        }
    }
}
