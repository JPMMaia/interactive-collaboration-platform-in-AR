using System;
using CollaborationEngine.Objects.Prefabs;

namespace CollaborationEngine.Objects
{
    public class TextInstruction : SceneObject
    {
        #region Properties
        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (IsInstanced)
                    GameObject.GetComponent<TextInstructionPrefab>().Text.text = value;
            }
        }
        #endregion

        #region Members
        private string _text;
        #endregion

        public TextInstruction() :
            base(ObjectLocator.Instance.TextInstructionPrefab, SceneObjectType.Indication)
        {
        }
    }
}
