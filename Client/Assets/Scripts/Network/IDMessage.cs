﻿using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class IDMessage : MessageBase
    {
        public uint ID { get; set; }

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