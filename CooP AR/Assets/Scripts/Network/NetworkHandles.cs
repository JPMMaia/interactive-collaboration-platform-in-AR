using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class NetworkHandles
    {
        public static short Initialize = MsgType.Highest + 1;
        public static short PresentStep = MsgType.Highest + 2;
        public static short UpdateHintTransform = MsgType.Highest + 3;
        public static short NeedMoreInstructions = MsgType.Highest + 4;
        public static short StepCompleted = MsgType.Highest + 5;
    }
}
