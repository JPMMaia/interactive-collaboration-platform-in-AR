using System;
using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.Networking;
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
        private bool _okClicked;
        private bool _instructionInstantiated;
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

            foreach (var group in ObjectLocator.Instance.UICanvas.GetComponentsInChildren<CanvasGroup>())
                group.interactable = false;
            GetComponent<CanvasGroup>().interactable = true;

            ObjectLocator.Instance.HintText.Enable(false);
        }

        public void Update()
        {
            if (!_okClicked || _instructionInstantiated)
                return;

            if (!Input.GetKeyDown(KeyCode.Mouse0))
                return;

            var selectedCamera = ObjectLocator.Instance.CameraManager.SelectedCamera.UnityCamera;
            var ray = selectedCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (!Physics.Raycast(ray, out hitInfo))
                return;

            var worldPosition = hitInfo.point;
            var root = ObjectLocator.Instance.SceneRoot;
            var worldToLocalMatrix = root.transform.worldToLocalMatrix;
            var localPosition = 0.1f * hitInfo.normal + worldToLocalMatrix.MultiplyPoint(worldPosition);

            SceneObject instruction;
            if (SelectedInstructionType.Type == InstructionType.Text)
            {
                instruction = new TextInstruction
                {
                    Name = NameInputField.text
                };
            }
            else
            {
                instruction = new TextureInstruction
                {
                    InstructionType = SelectedInstructionType.Type,
                    Name = NameInputField.text
                };
            }

            instruction.Position = localPosition;
            instruction.Rotation = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
            instruction.Scale = Vector3.one;

            Step.AddInstruction(instruction);

            _instructionInstantiated = true;

            foreach (var group in ObjectLocator.Instance.UICanvas.GetComponentsInChildren<CanvasGroup>())
                group.interactable = true;

            ObjectLocator.Instance.HintText.Enable(false);

            // Send new instruction:
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.AddInstruction, new SceneObject.DataMessage{ Data = instruction} );

            Destroy(gameObject);
        }

        #region Unity UI Event Handlers

        public void OnOKClicked()
        {
            if (NameInputField.text.Length == 0 || SelectedInstructionType == null)
                return;

            GetComponent<CanvasRenderer>().cull = true;
            foreach (var canvasRenderer in GetComponentsInChildren<CanvasRenderer>())
                canvasRenderer.cull = true;

            _okClicked = true;

            ObjectLocator.Instance.HintText.Enable(true);
            ObjectLocator.Instance.HintText.SetText("Click on a surface to create instruction.");
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