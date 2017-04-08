using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class Step
    {
        public class StepMessage : MessageBase
        {
            public Step Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
            }
            public override void Deserialize(NetworkReader reader)
            {
            }
        }
    }
}
