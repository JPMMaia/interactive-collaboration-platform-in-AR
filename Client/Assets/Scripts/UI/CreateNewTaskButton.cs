using CollaborationEngine.Objects;
using CollaborationEngine.States;
using CollaborationEngine.Tasks;
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
