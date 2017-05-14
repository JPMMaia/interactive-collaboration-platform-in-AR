using CollaborationEngine.Hints;
using CollaborationEngine.Shaders;
using CollaborationEngine.UserInterface;
using UnityEngine;

namespace CollaborationEngine.Base
{
    public class View : Entity
    {
        public Canvas MainCanvas;
        public ImageHintTextures ImageHintTextures;
        public Icons Icons;
        public GameObject SceneRoot;
        public ShaderLocator Shaders;
    }
}
