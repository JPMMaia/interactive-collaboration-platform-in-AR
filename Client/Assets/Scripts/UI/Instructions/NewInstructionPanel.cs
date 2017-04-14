using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Instructions
{
    public class NewInstructionPanel : MonoBehaviour
    {
        #region Unity UI
        public InputField NameInputField;
        public VerticalPanel InstructionTypeItemsContainer;
        public InstructionTypeItem InstructionTypeItemPrefab;
        #endregion

        #region Properties
        public Step Step { get; set; }
        private InstructionTypeItem SelectedInstructionType
        {
            get { return _selectedInstructionType; }
            set
            {
                if (_selectedInstructionType != null)
                    _selectedInstructionType.SetSelectedAppearance(false);

                _selectedInstructionType = value;

                if (_selectedInstructionType != null)
                    _selectedInstructionType.SetSelectedAppearance(true);
            }
        }
        #endregion

        #region Members
        private InstructionTypeItem _selectedInstructionType;
        #endregion

        public void Start()
        {
            for (var instructionType = 0; instructionType < (int)InstructionType.Count; ++instructionType)
            {
                var instructionTypeItem = Instantiate(InstructionTypeItemPrefab);
                instructionTypeItem.Type = (InstructionType)instructionType;
                instructionTypeItem.OnPressed += InstructionTypeItem_OnPressed;
                InstructionTypeItemsContainer.Add(instructionTypeItem.GetComponent<RectTransform>());
            }
        }

        #region Unity UI Event Handlers
        public void OnOKClicked()
        {
            if (NameInputField.text.Length == 0 || SelectedInstructionType == null)
                return;

            var instruction = new InstructionObject
            {
                InstructionType = SelectedInstructionType.Type,
                Name = NameInputField.text
            };
            Step.AddInstruction(instruction);

            Destroy(gameObject);
        }
        #endregion

        #region Event Handlers
        private void InstructionTypeItem_OnPressed(InstructionTypeItem sender, EventArgs eventArgs)
        {
            SelectedInstructionType = sender;
        }
        #endregion
    }
}
