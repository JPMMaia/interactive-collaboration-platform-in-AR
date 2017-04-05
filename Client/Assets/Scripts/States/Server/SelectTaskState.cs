using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class SelectTaskState : IApplicationState
    {
        public SelectTaskState(ServerCollaborationState serverState)
        {
            _serverState = serverState;
        }

        public void Initialize()
        {
            Debug.Log("Initialize SelectTaskState");

            // TODO Add tasks to panel
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown SelectTaskState");

            // TODO Remove tasks from panel
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }

        private readonly ServerCollaborationState _serverState;
    }
}
