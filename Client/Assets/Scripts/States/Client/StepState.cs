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
            networkManager.RegisterHandler(NetworkHandles.AddInstruction, OnAddInstruction);
            networkManager.RegisterHandler(NetworkHandles.RemoveInstruction, OnRemoveInstruction);
            networkManager.RegisterHandler(NetworkHandles.UpdateInstruction, OnUpdateInstruction);

            // Instantiate instructions:
            foreach (var instruction in _step.Instructions)
                instruction.Instantiate(ObjectLocator.Instance.SceneRoot.transform);

            ObjectLocator.Instance.HintText.SetText("Follow the mentor's instructions");
        }
        public void Shutdown()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.UnregisterHandler(NetworkHandles.UpdateInstruction);
            networkManager.UnregisterHandler(NetworkHandles.RemoveInstruction);
            networkManager.UnregisterHandler(NetworkHandles.AddInstruction);
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
        private void OnAddInstruction(NetworkMessage networkMessage)
        {

        }
        private void OnRemoveInstruction(NetworkMessage networkMessage)
        {
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {   
        }
    }
}
