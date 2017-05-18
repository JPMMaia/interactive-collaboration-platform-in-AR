using CollaborationEngine.Steps;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        private StepModel _currentStepModel;

        public void Awake()
        {
            NetworkServer.RegisterHandler(NetworkHandles.Initialize, OnInitialize);
            NetworkServer.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
            NetworkServer.RegisterHandler(NetworkHandles.UpdateHintTransform, OnUpdateInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.NeedMoreInstructions, OnNeedMoreInstructions);
            NetworkServer.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompleted);
        }

        private void OnInitialize(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.Initialize, new StepModelNetworkMessage(_currentStepModel));
        }
        private void OnPresentStep(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<StepModelNetworkMessage>();
            _currentStepModel = message.Data;

            NetworkServer.SendToAll(NetworkHandles.PresentStep, message);
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.UpdateHintTransform, networkMessage.ReadMessage<TransformNetworkMessage>());
        }
        private void OnNeedMoreInstructions(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.NeedMoreInstructions, networkMessage.ReadMessage<IDMessage>());
        }
        private void OnStepCompleted(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.StepCompleted, networkMessage.ReadMessage<IDMessage>());
        }
    }
}
