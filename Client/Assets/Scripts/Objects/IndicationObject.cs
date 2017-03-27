using System;
using System.Collections.Generic;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class IndicationObject : SceneObject
    {
        private static Dictionary<IndicationType, String> _textures = new Dictionary<IndicationType, string>();

        static IndicationObject()
        {
            _textures.Add(IndicationType.Arrow, "Textures/Arrows/011-right-arrow");
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
