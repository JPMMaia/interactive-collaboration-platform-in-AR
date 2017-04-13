using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Steps
{
    public class EditStepPanel : MonoBehaviour
    {
        #region UnityEditor
        public InputField StepNameInputField;
        #endregion

        #region Properties
        public Task Task { get; set; }
        public Step Step { get; set; }
        #endregion

        public void Start()
        {
            transform.SetParent(ObjectLocator.Instance.UICanvas, false);

            if (Step != null)
                StepNameInputField.text = Step.Name;

            StepNameInputField.ActivateInputField();
        }

        #region Unity UI Events
        public void OnOKClick()
        {
            if (Step != null)
            {
                Step.Name = StepNameInputField.text;
            }
            else
            {
                Task.AddStep(StepNameInputField.text);
            }

            Destroy(gameObject);
        }
        #endregion
    }
}
