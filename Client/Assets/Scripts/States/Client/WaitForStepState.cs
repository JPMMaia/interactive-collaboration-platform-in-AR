namespace CollaborationEngine.States.Client
{
    public class WaitForStepState : IApplicationState
    {
        #region Members
        private ClientCollaborationState _clientState;
        #endregion

        public WaitForStepState(ClientCollaborationState clientState)
        {
            _clientState = clientState;
        }

        public void Initialize()
        {
        }
        public void Shutdown()
        {
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }
    }
}
