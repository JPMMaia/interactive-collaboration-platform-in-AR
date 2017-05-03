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
            NetworkServer.RegisterHandler(NetworkHandles.AddInstruction, OnAddInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.RemoveInstruction, OnRemoveInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.UpdateInstruction, OnUpdateInstruction);
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.PresentStep, networkMessage.ReadMessage<GenericNetworkMessage<Step>>());
        }
        private void OnAddInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.AddInstruction, networkMessage.ReadMessage<SceneObject.SceneObjectMessage>());
        }
        private void OnRemoveInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.RemoveInstruction, networkMessage.ReadMessage<SceneObject.SceneObjectMessage>());
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.UpdateInstruction, networkMessage.ReadMessage<SceneObject.SceneObjectMessage>());
        }
    }
}
