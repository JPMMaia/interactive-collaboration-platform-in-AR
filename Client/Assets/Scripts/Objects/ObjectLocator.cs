using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class ObjectLocator : MonoBehaviour
    {
        public UnityEngine.Camera MainCamera
        {
            get { return UnityEngine.Camera.main; }
        }
        public GameObject SceneRoot;
        public GameObject StudyObjectPrefab;

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