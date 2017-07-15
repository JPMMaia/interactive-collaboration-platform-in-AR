using System.IO;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace CollaborationEngine.Steps
{
    public class StepModelNetworkMessage : MessageBase
    {
        public int ImageTargetIndex { get; private set; }
        public uint StepOrder { get; private set; }
        public StepModel Data { get; private set; }

        public StepModelNetworkMessage()
        {
        }
        public StepModelNetworkMessage(int imageTargetIndex, uint stepOrder, StepModel data)
        {
            ImageTargetIndex = imageTargetIndex;
            StepOrder = stepOrder;
            Data = data;
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(ImageTargetIndex);
            writer.Write(StepOrder);

            var data = new MemoryStream();
            Data.Serialize(new BinaryWriter(data));
            writer.Write(data.ToArray(), (int)data.Position);
        }
        public override void Deserialize(NetworkReader reader)
        {
            ImageTargetIndex = reader.ReadInt32();
            StepOrder = reader.ReadUInt32();

            var application = Object.FindObjectOfType<Base.Application>();
            Data = Object.Instantiate(application.Prefabs.StepModelPrefab);
            var data = new MemoryStream(reader.ReadBytes(reader.Length - 8));
            Data.Deserialize(new BinaryReader(data));
        }
    }
}
