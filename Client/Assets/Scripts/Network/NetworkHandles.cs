using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class NetworkHandles
    {
        public static short AddTaskOnServerHandle = MsgType.Highest + 8;
        public static short AddTaskOnClientHandle = MsgType.Highest + 9;
        public static short RemoveTaskOnServerHandle = MsgType.Highest + 10;
        public static short RemoveTaskOnClientHandle = MsgType.Highest + 11;
        public static short EditTaskOnServerHandle = MsgType.Highest + 12;
        public static short EditTaskOnClientHandle = MsgType.Highest + 13;
        public static short UpdateTaskOnServerHandle = MsgType.Highest + 14;
        public static short UpdateTaskOnClientHandle = MsgType.Highest + 15;
    }
}
