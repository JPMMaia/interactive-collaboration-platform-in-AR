using CollaborationEngine.Objects;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class CreateNewTaskButton : MonoBehaviour
    {
        public void OnClicked()
        {
            Instantiate(ObjectLocator.Instance.EditTaskPanelPrefab);
        }
    }
}
