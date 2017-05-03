using CollaborationEngine.Tasks;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        public void Awake()
        {
            NetworkServer.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.PresentStep, networkMessage.ReadMessage<GenericNetworkMessage<Step>>());
        }
    }
}
