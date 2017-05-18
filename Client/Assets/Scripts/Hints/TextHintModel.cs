using System.IO;
using UnityEngine;

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

        public override HintModel DeepCopy(Transform parent, uint taskID, uint stepID)
        {
            var hint = Instantiate(this, parent);

            DeepCopy(hint, taskID, stepID);

            return hint;
        }
    }
}
