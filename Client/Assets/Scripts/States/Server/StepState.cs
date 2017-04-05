using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class StepState : IApplicationState
    {
        public StepState(ServerCollaborationState serverState, Task task)
        {
            _serverState = serverState;
            _task = task;
        }

        public void Initialize()
        {
            Debug.Log("Initialize StepState");
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown StepState");
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }

        private readonly ServerCollaborationState _serverState;
        private readonly Task _task;
    }
}
