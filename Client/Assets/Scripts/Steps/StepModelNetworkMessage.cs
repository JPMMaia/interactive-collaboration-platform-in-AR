using System.IO;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace CollaborationEngine.Steps
{
    public class StepModelNetworkMessage : MessageBase
    {
        public StepModel Data { get; set; }

        public StepModelNetworkMessage()
        {
        }
        public StepModelNetworkMessage(StepModel data)
        {
            Data = data;
        }

        public override void Serialize(NetworkWriter writer)
        {
            var data = new MemoryStream();
            Data.Serialize(new BinaryWriter(data));

            writer.Write(data.GetBuffer(), (int)data.Position);
        }
        public override void Deserialize(NetworkReader reader)
        {
            var application = Object.FindObjectOfType<Base.Application>();
            Data = Object.Instantiate(application.Prefabs.StepModelPrefab);

            var data = new MemoryStream(reader.ReadBytes(reader.Length));
            Data.Deserialize(new BinaryReader(data));
        }
    }
}
