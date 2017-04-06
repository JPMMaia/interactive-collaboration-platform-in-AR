using System;
using CollaborationEngine.States;
using CollaborationEngine.States.Server;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    public class TaskItem : MonoBehaviour
    {
        public String TaskName { get; set; }

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
            // TODO
        }

        public void OnDeleteClick()
        {
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                var serverState = currentState as ServerCollaborationState;
                serverState.TaskManager.RemoveTask(TaskName);
            }
        }
    }
}
