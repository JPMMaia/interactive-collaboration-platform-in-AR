using CollaborationEngine.Base;
using CollaborationEngine.Utilities;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class ImageHint3DView : Entity
    {
        public Texture Image
        {
            get { return _image; }
            set
            {
                _image = value;

                var meshRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in meshRenderers)
                {
                    var options = AssetsUtils.MaterialOptions.None;
                    if (meshRenderer.gameObject.CompareTag("Backface"))
                        options = (AssetsUtils.MaterialOptions)((uint)options | (uint)AssetsUtils.MaterialOptions.InvertX);

                    AssetsUtils.SetTexturedMaterial(meshRenderer, value, Application.View.Shaders.TransparentCutoutShader, options);
                }
            }
        }

        private Texture _image;
    }
}
