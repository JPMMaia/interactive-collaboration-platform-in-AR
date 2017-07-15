using System;
using UnityEngine;

namespace CollaborationEngine.Utilities
{
    public class AssetsUtils
    {
        public enum MaterialOptions
        {
            None = 0,
            InvertX = 1
        }

        public static Texture LoadTexture(String path)
        {
            return Resources.Load(path, typeof(Texture)) as Texture;
        }

        public static Material CreateTexturedMaterial(Texture texture, Shader shader)
        {
            // CreateStep material:
            var material = new Material(shader);

            // Set texture:
            material.SetTexture("_MainTex", texture);

            return material;
        }

        public static void SetTexturedMaterial(MeshRenderer meshRenderer, Texture texture, Shader shader, MaterialOptions options)
        {
            // Set textured material:
            var materials = meshRenderer.materials;

            var material = CreateTexturedMaterial(texture, shader);

            if (((uint)options & (uint)MaterialOptions.InvertX) != 0)
                material.mainTextureScale = new Vector2(-1.0f, 1.0f);

            materials[0] = material;
            meshRenderer.materials = materials;
        }
        public static void SetTexturedMaterial(MeshRenderer meshRenderer, String texturePath, Shader shader, MaterialOptions options)
        {
            // Load texture:
            var texture = LoadTexture(texturePath);

            SetTexturedMaterial(meshRenderer, texture, shader, options);
        }
    }
}