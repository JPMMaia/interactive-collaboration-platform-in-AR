using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class IDNetworkMessage : MessageBase
    {
        public uint ID { get; private set; }

        public IDNetworkMessage()
        {
        }
        public IDNetworkMessage(uint id)
        {
            ID = id;
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(ID);
        }
        public override void Deserialize(NetworkReader reader)
        {
            ID = reader.ReadPackedUInt32();
        }
    }
}
