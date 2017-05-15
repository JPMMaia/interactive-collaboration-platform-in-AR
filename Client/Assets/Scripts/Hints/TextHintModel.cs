using System.IO;

namespace CollaborationEngine.Hints
{
    public class TextHintModel : HintModel
    {
        public TextHintModel()
        {
            Type = HintType.Text;
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);

            Type = HintType.Text;
        }
    }
}
