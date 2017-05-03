using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine.Networking;

namespace CollaborationEngine.States.Client
{
    public class StepState : IApplicationState
    {
        #region Members
        private readonly ClientCollaborationState _clientState;
        private readonly Step _step;
        #endregion

        public StepState(ClientCollaborationState clientState, Step step)
        {
            _clientState = clientState;
            _step = step;
        }

        public void Initialize()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.RegisterHandler(NetworkHandles.PresentStep, OnChangeStep);

            // TODO subscribe to instructions changes: addition, deletion and updates
            
            // Instantiate instructions:
            foreach (var instruction in _step.Instructions)
                instruction.Instantiate(ObjectLocator.Instance.SceneRoot.transform);

            ObjectLocator.Instance.HintText.SetText("Follow the mentor's instructions");
        }
        public void Shutdown()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.UnregisterHandler(NetworkHandles.PresentStep);
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }

        private void OnChangeStep(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<GenericNetworkMessage<Step>>();
            _clientState.CurrentState = new StepState(_clientState, message.Data);
        }
    }
}
