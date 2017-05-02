using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        public void Awake()
        {
            NetworkServer.RegisterHandler(NetworkHandles.AddTaskHandle, OnAddTask);
            NetworkServer.RegisterHandler(NetworkHandles.RemoveTaskHandle, OnRemoveTask);
            NetworkServer.RegisterHandler(NetworkHandles.UpdateTaskHandle, OnUpdateTask);
        }

        private ServerController()
        {
        }

        private void OnAddTask(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.AddTaskHandle, networkMessage.ReadMessage<Task.TaskMesssage>());
        }
        private void OnRemoveTask(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.RemoveTaskHandle, networkMessage.ReadMessage<Task.TaskMesssage>());
        }
        private void OnUpdateTask(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.UpdateTaskHandle, networkMessage.ReadMessage<Task.TaskMesssage>());
        }
    }
}
