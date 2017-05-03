using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class NetworkHandles
    {
        public static short PresentStep = MsgType.Highest + 1;
        public static short AddInstruction = MsgType.Highest + 2;
        public static short RemoveInstruction = MsgType.Highest + 3;
        public static short UpdateInstruction = MsgType.Highest + 4;
    }
}
