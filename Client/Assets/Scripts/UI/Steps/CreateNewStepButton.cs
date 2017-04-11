using CollaborationEngine.Objects;
using UnityEngine;

namespace CollaborationEngine.UI.Steps
{
    public class CreateNewStepButton : MonoBehaviour
    {
        public void OnClicked()
        {
            Instantiate(ObjectLocator.Instance.EditStepPanelPrefab);
        }
    }
}
