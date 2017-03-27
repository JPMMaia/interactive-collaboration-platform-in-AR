using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class IndicationObject : SceneObject
    {
        public IndicationObject(Data networkData) : 
            base(ObjectLocator.Instance.IndicationPrefab, networkData)
        {
        }
    }
}
