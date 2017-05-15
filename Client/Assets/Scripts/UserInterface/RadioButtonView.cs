using CollaborationEngine.Base;
using UnityEngine.UI;

namespace CollaborationEngine.UserInterface
{
    public class RadioButtonView : Entity
    {
        public RawImage SelectedRawImage;

        public bool Selected
        {
            get
            {
                return SelectedRawImage.gameObject.activeInHierarchy;
            }
            set
            {
                SelectedRawImage.gameObject.SetActive(value);
            }
        }
    }
}
