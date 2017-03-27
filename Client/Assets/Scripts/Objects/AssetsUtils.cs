using System;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class AssetsUtils
    {
        public enum MaterialOptions
        {
            None = 0,
            InvertX = 1,
            Cutout = 2
        }

        public static Texture LoadTexture(String path)
        {
            return Resources.Load(path, typeof(Texture)) as Texture;
        }

        public static Material CreateTexturedMaterial(Texture texture)
        {
            // Create material:
            var material = new Material(Shader.Find("Standard"));

            // Set texture:
            material.SetTexture("_MainTex", texture);

            return material;
        }

        public static void SetTexturedMaterial(MeshRenderer meshRenderer, String texturePath, MaterialOptions options)
        {
            // Load texture:
            var texture = LoadTexture(texturePath);

            // Create and set textured material:
            var materials = meshRenderer.materials;

            var material = CreateTexturedMaterial(texture);

            if(((uint)options & (uint)MaterialOptions.InvertX) != 0)
                material.mainTextureScale = new Vector2(-1.0f, 1.0f);

            if (((uint) options & (uint) MaterialOptions.Cutout) != 0)
            {
                material.SetFloat("_Mode", 1);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }

            materials[0] = material;
            meshRenderer.materials = materials;
        }
    }
}