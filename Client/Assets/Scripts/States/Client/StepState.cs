using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.States.Client
{
    public class StepState : IApplicationState
    {
        public StepState(ClientCollaborationState clientState, Task task)
        {
            _clientState = clientState;
            _task = task;
        }

        public void Initialize()
        {
            Debug.Log("Initialize StepState");
        }
        public void Shutdown()
        {
            Debug.Log("´Shutdown StepState");
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }

        private readonly ClientCollaborationState _clientState;
        private readonly Task _task;
    }
}
