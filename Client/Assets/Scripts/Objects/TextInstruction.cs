using System;
using CollaborationEngine.Network;
using CollaborationEngine.Objects.Prefabs;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Objects
{
    public class TextInstruction : SceneObject
    {
        #region Properties

        public override Type Type
        {
            get { return typeof(TextInstruction); }
        }
        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _needsSynch = true;
                if (IsInstanced)
                    GameObject.GetComponent<TextInstructionPrefab>().Text.text = value;
            }
        }

        #endregion

        #region Members
        private string _text;
        private bool _needsSynch;
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

        public override void Update(SceneObject instruction)
        {
            base.Update(instruction);

            var textInstruction = (TextInstruction) instruction;
            Text = textInstruction.Text;
        }

        public override bool PerformNetworkSynch()
        {
            if (!base.PerformNetworkSynch())
            {
                if (!_needsSynch)
                    return false;

                var networkClient = NetworkManager.singleton.client;
                networkClient.Send(NetworkHandles.UpdateHintTransform, new DataMessage { Data = this });
                _needsSynch = false;
                return true;
            }

            return true;
        }
    }
}
