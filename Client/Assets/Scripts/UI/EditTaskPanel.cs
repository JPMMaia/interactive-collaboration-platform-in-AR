using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    public class EditTaskPanel : MonoBehaviour
    {
        public void OnOKClick()
        {
            Task.Name = TaskNameInputField.text;

            Destroy();
        }

        public Task Task { get; set; }

        public InputField TaskNameInputField;

        private void Destroy()
        {
            Task = null;

            Destroy(gameObject);
        }
    }
}
