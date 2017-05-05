using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine.Networking;

namespace CollaborationEngine.States.Client
{
    public class WaitForStepState : IApplicationState
    {
        #region Members
        private readonly ClientCollaborationState _clientState;
        #endregion

        public WaitForStepState(ClientCollaborationState clientState)
        {
            _clientState = clientState;
        }

        public void Initialize()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);

            ObjectLocator.Instance.HintText.SetText("Waiting for mentor");
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
        public void LateUpdate()
        {
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<GenericNetworkMessage<Step>>();
            _clientState.CurrentState = new StepState(_clientState, message.Data);
        }
    }
}
