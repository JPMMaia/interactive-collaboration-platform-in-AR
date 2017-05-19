using System;
using System.Linq;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public enum ImageHintType : byte
    {
        ArrowUp,
        ArrowDown,
        ArrowLeft,
        ArrowRight,
        RotateClockwise,
        RotateCounterclockwise,
        Wrench,
        Axe,
        Count,
        Null
    }

    public class ImageHintTextures : Entity
    {
        [Serializable]
        public struct ImageHintTextureKeyValuePair
        {
            public ImageHintType Type;
            public Texture Texture;
        }

        public ImageHintTextureKeyValuePair[] HintTextures;

        public Texture GetTexture(ImageHintType type)
        {
            return HintTextures.First(pair => pair.Type == type).Texture;
        }
    }
}
