using CollaborationEngine.Feedback;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        public void Awake()
        {
            NetworkServer.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
            NetworkServer.RegisterHandler(NetworkHandles.StopPresentStep, OnStopPresentStep);
            NetworkServer.RegisterHandler(NetworkHandles.AddInstruction, OnAddInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.RemoveInstruction, OnRemoveInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.UpdateInstruction, OnUpdateInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.HelpWanted, OnHelpWanted);
            NetworkServer.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompleted);
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.PresentStep, networkMessage.ReadMessage<GenericNetworkMessage<Step>>());
        }
        private void OnStopPresentStep(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.StopPresentStep, networkMessage.ReadMessage<IDMessage>());
        }
        private void OnAddInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.AddInstruction, networkMessage.ReadMessage<SceneObject.DataMessage>());
        }
        private void OnRemoveInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.RemoveInstruction, networkMessage.ReadMessage<SceneObject.IDMessage>());
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.UpdateInstruction, networkMessage.ReadMessage<SceneObject.DataMessage>());
        }
        private void OnHelpWanted(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.HelpWanted, networkMessage.ReadMessage<ApprenticeFeedbackModule.StringMessage>());
        }
        private void OnStepCompleted(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.StepCompleted, networkMessage.ReadMessage<ApprenticeFeedbackModule.StringMessage>());
        }
    }
}
