using System.IO;

namespace CollaborationEngine.Hints
{
    public class ImageHintModel : HintModel
    {
        public ImageHintType ImageHintType { get; set; }

        public ImageHintModel()
        {
            Type = HintType.Image;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write((byte) ImageHintType);
        }
        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);

            ImageHintType = (ImageHintType) reader.ReadByte();
        }
    }
}
