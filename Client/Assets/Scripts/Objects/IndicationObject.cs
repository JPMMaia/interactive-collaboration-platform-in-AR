using System;
using System.Collections.Generic;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class IndicationObject : SceneObject
    {
        private static readonly Dictionary<IndicationType, String> Textures = new Dictionary<IndicationType, string>();

        static IndicationObject()
        {
            Textures.Add(IndicationType.Arrow, "Textures/Arrows/011-right-arrow");
            Textures.Add(IndicationType.RotateClockwise, "Textures/Arrows/007-refresh-button");
            Textures.Add(IndicationType.RotateCounterclockwise, "Textures/Arrows/012-reverse-refresh-button");
            Textures.Add(IndicationType.Wrench, "Textures/Tools/004-shape");
            Textures.Add(IndicationType.Axe, "Textures/Tools/001-cut");
            Textures.Add(IndicationType.Screwer, "Textures/Tools/005-tool-2");
            Textures.Add(IndicationType.Hammer, "Textures/Tools/007-tool");
        }

        public IndicationObject(Data networkData) : 
            base(ObjectLocator.Instance.IndicationPrefab, networkData)
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

                AssetsUtils.SetTexturedMaterial(meshRenderer, Textures[(IndicationType) NetworkData.Flag], options);
            }

            return gameObject;
        }
    }
}
