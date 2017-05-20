using CollaborationEngine.AugmentedReality;
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
        public ShaderLocator Shaders;
        public ImageTargetCollection ImageTargets;

        public GameObject SceneRoot
        {
            get { return ImageTargets.ActivatedSceneRoot; }
        }
    }
}
