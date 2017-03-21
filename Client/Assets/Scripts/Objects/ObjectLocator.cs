using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class ObjectLocator : MonoBehaviour
    {
        public Transform SceneRootTransform;
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