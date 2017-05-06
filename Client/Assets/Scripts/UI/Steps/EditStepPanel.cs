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
        public TaskModel TaskModel { get; set; }
        public StepModel StepModel { get; set; }
        #endregion

        public void Start()
        {
            transform.SetParent(ObjectLocator.Instance.UICanvas, false);

            if (StepModel != null)
                StepNameInputField.text = StepModel.Name;

            StepNameInputField.ActivateInputField();
        }

        #region Unity UI Events
        public void OnOKClick()
        {
            if (StepModel != null)
            {
                StepModel.Name = StepNameInputField.text;
            }
            else
            {
                TaskModel.AddStep(StepNameInputField.text);
            }

            Destroy(gameObject);
        }
        #endregion
    }
}
