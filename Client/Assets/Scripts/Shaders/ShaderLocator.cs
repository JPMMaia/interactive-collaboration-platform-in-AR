using CollaborationEngine.Objects;
using UnityEngine;

namespace CollaborationEngine.Shaders
{
    public class ShaderLocator : MonoBehaviour
    {
        public static ShaderLocator Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<ShaderLocator>();

                return _instance;
            }
        }

        public Shader StandardShader;
        public Shader TransparentCutoutShader;

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private ShaderLocator()
        {
        }

        private static ShaderLocator _instance;
    }
}
