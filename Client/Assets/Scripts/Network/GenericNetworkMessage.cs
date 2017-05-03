using System;
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
            Data.Serialize(writer);
        }
        public override void Deserialize(NetworkReader reader)
        {
            var type = typeof(TData);
            Data = (TData)Activator.CreateInstance(type);

            Data.Deserialize(reader);
        }
    }
}
