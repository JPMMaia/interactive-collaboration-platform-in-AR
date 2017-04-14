using System;
using System.Collections.Generic;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class InstructionObject : SceneObject
    {
        private static readonly Dictionary<InstructionType, String> Textures = new Dictionary<InstructionType, string>();

        static InstructionObject()
        {
            Textures.Add(InstructionType.Arrow, "Textures/Arrows/011-right-arrow");
            Textures.Add(InstructionType.RotateClockwise, "Textures/Arrows/007-refresh-button");
            Textures.Add(InstructionType.RotateCounterclockwise, "Textures/Arrows/012-reverse-refresh-button");
            Textures.Add(InstructionType.Wrench, "Textures/Tools/004-shape");
            Textures.Add(InstructionType.Axe, "Textures/Tools/001-cut");
            Textures.Add(InstructionType.Screwer, "Textures/Tools/005-tool-2");
            Textures.Add(InstructionType.Hammer, "Textures/Tools/007-tool");
        }

        public InstructionType InstructionType { get; set; }

        public InstructionObject() : 
            base(ObjectLocator.Instance.IndicationPrefab, SceneObjectType.Indication)
        {
        }

        public override GameObject Instantiate(Transform parent)
        {
            var gameObject = base.Instantiate(parent);

            var meshRenderers = GameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                var options = AssetsUtils.MaterialOptions.Cutout;
                if (meshRenderer.gameObject.CompareTag("Backface"))
                    options = (AssetsUtils.MaterialOptions)((uint)options | (uint)AssetsUtils.MaterialOptions.InvertX);

                AssetsUtils.SetTexturedMaterial(meshRenderer, Textures[InstructionType], options);
            }

            return gameObject;
        }
    }
}
