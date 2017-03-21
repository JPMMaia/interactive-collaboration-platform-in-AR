using System;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class AssetsUtils
    {
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

        public static void SetTexturedMaterial(MeshRenderer meshRenderer, String texturePath)
        {
            // Load texture:
            var texture = LoadTexture(texturePath);

            // Create and set textured material:
            var materials = meshRenderer.materials;
            materials[0] = CreateTexturedMaterial(texture);
            meshRenderer.materials = materials;
        }
    }
}