using System;
using System.Collections.Generic;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class IndicationObject : SceneObject
    {
        private static readonly Dictionary<IndicationType, String> _textures = new Dictionary<IndicationType, string>();

        static IndicationObject()
        {
            _textures.Add(IndicationType.Arrow, "Textures/Arrows/011-right-arrow");
            _textures.Add(IndicationType.RotateClockwise, "Textures/Arrows/007-refresh-button");
            _textures.Add(IndicationType.RotateCounterclockwise, "Textures/Arrows/012-reverse-refresh-button");
            _textures.Add(IndicationType.Wrench, "Textures/Tools/004-shape");
            _textures.Add(IndicationType.Axe, "Textures/Tools/001-cut");
            _textures.Add(IndicationType.Screwer, "Textures/Tools/005-tool-2");
            _textures.Add(IndicationType.Hammer, "Textures/Tools/007-tool");
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

                AssetsUtils.SetTexturedMaterial(meshRenderer, _textures[(IndicationType) NetworkData.Flag], options);
            }

            return gameObject;
        }
    }
}
