using CollaborationEngine.Base;

namespace CollaborationEngine.AugmentedReality
{
    public class HideInApprentice : Entity
    {
        public void Awake()
        {
            if (Application.IsApprentice)
                gameObject.SetActive(false);
        }
    }
}
