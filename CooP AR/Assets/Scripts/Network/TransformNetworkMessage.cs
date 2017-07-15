using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class TransformNetworkMessage : MessageBase
    {
        public uint ID { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public TransformNetworkMessage()
        {
        }
        public TransformNetworkMessage(uint id, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            ID = id;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(ID);
            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(Scale);
        }
        public override void Deserialize(NetworkReader reader)
        {
            ID = reader.ReadUInt32();
            Position = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();
            Scale = reader.ReadVector3();
        }
    }
}
