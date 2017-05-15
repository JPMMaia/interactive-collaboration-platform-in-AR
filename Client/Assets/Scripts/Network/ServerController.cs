using CollaborationEngine.Steps;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        public void Awake()
        {
            NetworkServer.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
            NetworkServer.RegisterHandler(NetworkHandles.UpdateHintTransform, OnUpdateInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.NeedMoreInstructions, OnNeedMoreInstructions);
            NetworkServer.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompleted);
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.PresentStep, networkMessage.ReadMessage<StepModelNetworkMessage>());
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
