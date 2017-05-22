using System.IO;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class GeometryHintModel : HintModel
    {
        public uint ModelID { get; set; }

        public GeometryHintModel()
        {
            Type = HintType.Geometry;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(ModelID);
        }
        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);

            ModelID = reader.ReadUInt32();
        }

        public override HintModel DeepCopy(Transform parent, uint taskID, uint stepID)
        {
            var hint = Instantiate(this, parent);

            DeepCopy(hint, taskID, stepID);

            hint.ModelID = ModelID;

            return hint;
        }
    }
}
