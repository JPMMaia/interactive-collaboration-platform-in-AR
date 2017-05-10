using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Panels
{
    public class InstructionsView : Entity
    {
        public RectTransform Container;

        public void AddToContainer(Transform element)
        {
            element.SetParent(Container, false);
        }
        public void RemoveFromContainer(Transform element)
        {
            element.SetParent(null);
        }
    }
}
