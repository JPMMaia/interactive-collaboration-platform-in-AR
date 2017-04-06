using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class RemoveTaskButton : MonoBehaviour
    {
        public void OnClicked()
        {
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                var serverState = currentState as ServerCollaborationState;
                serverState.TaskManager.RemoveTask("Task 123");

                // TODO input text from user
            }
        }
    }
}
