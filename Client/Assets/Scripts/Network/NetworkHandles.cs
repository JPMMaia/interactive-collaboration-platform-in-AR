using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class NetworkHandles
    {
        public static short AddTaskHandle = MsgType.Highest + 1;
        public static short RemoveTaskHandle = MsgType.Highest + 2;
        public static short EditTaskHandle = MsgType.Highest + 3;
        public static short UpdateTaskHandle = MsgType.Highest + 4;
        public static short PresentStep = MsgType.Highest + 5;
    }
}
