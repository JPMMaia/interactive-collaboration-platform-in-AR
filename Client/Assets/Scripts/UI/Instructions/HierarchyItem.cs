using System;
using CollaborationEngine.Camera;
using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Instructions
{
    [RequireComponent(typeof(RectTransform))]
    public class HierarchyItem : MonoBehaviour
    {
        #region Delegates
        public delegate void IntructionItemEventDelegate(HierarchyItem sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event IntructionItemEventDelegate OnClicked;
        #endregion

        #region Unity Editor
        public EditInstructionPanel EditInstructionPanelPrefab;
        public Text IntructionNameText;
        public Image InstructionButtonImage;
        #endregion

        #region Properties
        public Step Step { get; set; }
        public SceneObject Instruction { get; set; }
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
        }
        #endregion

        #region Members
        private RectTransform _rectTransform;
        #endregion

        public void Start()
        {
            IntructionNameText.text = Instruction.Name;

            Instruction.Instantiate(ObjectLocator.Instance.SceneRoot.transform);
            Instruction.OnNameChanged += Instruction_OnNameChanged;
            Instruction.InputCollider.OnPressed += InputCollider_OnPressed;
        }
        public void OnDestroy()
        {
            Instruction.InputCollider.OnPressed -= InputCollider_OnPressed;
            Instruction.OnNameChanged -= Instruction_OnNameChanged;
            Instruction.Destroy();
        }

        public void Select()
        {
            SetSelectedAppearance(true);
            ToggleGizmo(true);
        }
        public void Unselect()
        {
            ToggleGizmo(false);
            SetSelectedAppearance(false);
        }

        private void SetSelectedAppearance(bool enable)
        {
            InstructionButtonImage.color = enable ? Color.green : Color.white;
        }
        private void ToggleGizmo(bool enable)
        {
            var instance = TransformGizmoManager.Instance;

            instance.Target = enable ? Instruction.GameObject.transform : null;
        }

        #region Unity UI Event Handlers
        public void OnItemClick()
        {
            if (OnClicked != null)
                OnClicked(this, EventArgs.Empty);
        }
        public void OnEditClick()
        {
            var editTaskPanel = Instantiate(EditInstructionPanelPrefab);
            editTaskPanel.Instruction = Instruction;
        }
        public void OnDeleteClick()
        {
            Step.RemoveInstruction(Instruction.ID);

            // Send remove instruction:
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.RemoveInstruction, new SceneObject.IDMessage { ID = Instruction.ID });
        }
        #endregion

        #region Event Handlers
        private void Instruction_OnNameChanged(SceneObject sender, EventArgs eventArgs)
        {
            IntructionNameText.text = sender.Name;
        }
        private void InputCollider_OnPressed(Objects.Components.InputColliderComponent<SceneObject> sender, EventArgs eventArgs)
        {
            OnItemClick();
        }
        #endregion
    }
}
