using System;
using CollaborationEngine.Objects.Prefabs;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Objects
{
    public class TextInstruction : SceneObject
    {
        #region Properties

        public override SceneObjectType Type
        {
            get
            {
                return SceneObjectType.Text;
            }
        }
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
            base(ObjectLocator.Instance.TextInstructionPrefab)
        {
        }

        public override GameObject Instantiate(Transform parent)
        {
            var gameObject = base.Instantiate(parent);

            Text = _text;

            return gameObject;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);

            writer.Write(_text);
        }
        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);

            _text = reader.ReadString();
        }
    }
}
