using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class ObjectLocator : MonoBehaviour
    {
        public UnityEngine.Camera MainCamera
        {
            get { return UnityEngine.Camera.main; }
        }

        public Shader StandardShader;

        public ClientController ClientController;

        public GameObject ServerRoot;
        public GameObject ClientRoot;
        public GameObject SceneRoot;

        public GameObject StudyObjectPrefab;
        public GameObject IndicationPrefab;
        public GameObject IndicationToolsPrefab;

        private static ObjectLocator _instance;
        public static ObjectLocator Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType(typeof(ObjectLocator)) as ObjectLocator;

                return _instance;
            }
        }

        private ObjectLocator()
        {
        }
    }
}