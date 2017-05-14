using System;
using System.Collections.Generic;
using CollaborationEngine.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Objects
{
    public class TextureInstruction : SceneObject
    {
        private static readonly Dictionary<InstructionType, String> Textures = new Dictionary<InstructionType, string>();

        static TextureInstruction()
        {
            Textures.Add(InstructionType.Arrow, "Textures/Arrows/011-right-arrow");
            Textures.Add(InstructionType.RotateClockwise, "Textures/Arrows/007-refresh-button");
            Textures.Add(InstructionType.RotateCounterclockwise, "Textures/Arrows/012-reverse-refresh-button");
            Textures.Add(InstructionType.Wrench, "Textures/Tools/004-shape");
            Textures.Add(InstructionType.Axe, "Textures/Tools/001-cut");
            Textures.Add(InstructionType.Screwer, "Textures/Tools/005-tool-2");
            Textures.Add(InstructionType.Hammer, "Textures/Tools/007-tool");
        }

        #region Properties
        public override Type Type
        {
            get { return typeof(TextureInstruction); }
        }
        #endregion

        public InstructionType InstructionType { get; set; }

        public TextureInstruction() :
            base(ObjectLocator.Instance.IndicationPrefab)
        {
        }

        public override GameObject Instantiate(Transform parent)
        {
            var gameObject = base.Instantiate(parent);

            var meshRenderers = GameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
               /* var options = AssetsUtils.MaterialOptions.Cutout;
                if (meshRenderer.gameObject.CompareTag("Backface"))
                    options = (AssetsUtils.MaterialOptions)((uint)options | (uint)AssetsUtils.MaterialOptions.InvertX);

                AssetsUtils.SetTexturedMaterial(meshRenderer, Textures[InstructionType], options);*/
            }

            return gameObject;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);

            writer.WritePackedUInt32((UInt32) InstructionType);
        }
        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);

            InstructionType = (InstructionType) reader.ReadPackedUInt32();
        }
    }
}
