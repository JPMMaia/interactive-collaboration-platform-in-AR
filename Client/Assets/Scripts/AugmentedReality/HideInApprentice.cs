using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.AugmentedReality
{
    public class HideInApprentice : Entity
    {
        public void Awake()
        {
            if (Application.IsApprentice)
                GetComponent<Renderer>().gameObject.SetActive(false);
        }
    }
}
