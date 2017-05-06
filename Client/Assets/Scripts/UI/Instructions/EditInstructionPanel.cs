using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Instructions
{
    public class EditInstructionPanel : MonoBehaviour
    {
        #region UnityEditor
        public InputField InstructionNameInputField;
        #endregion

        #region Properties
        public StepModel StepModel { get; set; }
        public SceneObject Instruction { get; set; }
        #endregion

        public void Start()
        {
            transform.SetParent(ObjectLocator.Instance.UICanvas, false);

            if (Instruction != null)
                InstructionNameInputField.text = Instruction.Name;

            InstructionNameInputField.ActivateInputField();
        }

        public void OnOKClick()
        {
            if (Instruction != null)
            {
                Instruction.Name = InstructionNameInputField.text;
            }
            else
            {
                StepModel.AddInstruction(new TextureInstruction());
            }

            Destroy(gameObject);
        }
    }
}
