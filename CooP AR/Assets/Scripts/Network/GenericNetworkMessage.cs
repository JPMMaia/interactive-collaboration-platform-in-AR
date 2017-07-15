using System;
using System.IO;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class GenericNetworkMessage<TData> : MessageBase where TData : ISerializable
    {
        public TData Data { get; set; }

        public GenericNetworkMessage()
        {
        }
        public GenericNetworkMessage(TData data)
        {
            Data = data;
        }

        public override void Serialize(NetworkWriter writer)
        {
            var data = new MemoryStream();
            Data.Serialize(new BinaryWriter(data));

            writer.Write(data.ToArray(), (int) data.Position);
        }
        public override void Deserialize(NetworkReader reader)
        {
            var type = typeof(TData);
            Data = (TData)Activator.CreateInstance(type);

            var data = new MemoryStream(reader.ReadBytes(reader.Length));
            Data.Deserialize(new BinaryReader(data));
        }
    }
}
