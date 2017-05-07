using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.Tasks.Steps;
using CollaborationEngine.UI.Inspector;
using UnityEngine;

namespace CollaborationEngine.UI.Instructions
{
    public class HierarchyPanel : MonoBehaviour
    {
        #region Unity UI
        public HierarchyItem HierarchyItemPrefab;
        public NewInstructionPanel NewInstructionPanelPrefab;
        public InspectorPanel InspectorPanelPrefab;
        public TextInspectorPanel TextInspectorPanelPrefab;
        public VerticalPanel Container;
        #endregion

        #region Properties
        public StepModel StepModel { get; set; }
        public HierarchyItem SelectedHierarchyItem
        {
            get { return _selectedHierarchyItem; }
            set
            {
                if (_selectedHierarchyItem != null)
                    _selectedHierarchyItem.Unselect();

                if (_selectedHierarchyItem == value)
                    value = null;

                _selectedHierarchyItem = value;

                if (_selectedHierarchyItem != null)
                {
                    _selectedHierarchyItem.Select();
                    CreateInspectorPanel();
                }
                else
                {
                    DestroyInspectorPanel();
                }
            }
        }

        #endregion

        #region Members
        private readonly List<HierarchyItem> _items = new List<HierarchyItem>();
        private HierarchyItem _selectedHierarchyItem;
        private GameObject _inspectorPanel;
        #endregion

        public void Start()
        {
            StepModel.OnInstructionAdded += Step_OnInstructionAdded;
            StepModel.OnInstructionRemoved += Step_OnInstructionRemoved;

            foreach (var instruction in StepModel.Instructions)
                AddInstructionItem(instruction);

            ObjectLocator.Instance.HintText.SetText("Create, delete or select an instruction.");
        }
        public void OnDestroy()
        {
            foreach (var instruction in StepModel.Instructions)
                RemoveInstructionItem(instruction);

            StepModel.OnInstructionRemoved -= Step_OnInstructionRemoved;
            StepModel.OnInstructionAdded -= Step_OnInstructionAdded;
        }

        private void CreateInspectorPanel()
        {
            DestroyInspectorPanel();

            if (SelectedHierarchyItem.Instruction is TextInstruction)
            {
                var panel = Instantiate(TextInspectorPanelPrefab);
                panel.TextInstruction = (TextInstruction) SelectedHierarchyItem.Instruction;
                _inspectorPanel = panel.gameObject;
            }
            else
            {
                _inspectorPanel = Instantiate(InspectorPanelPrefab).gameObject;
            }

            ObjectLocator.Instance.RightPanel.Add(_inspectorPanel.GetComponent<RectTransform>());
        }
        private void DestroyInspectorPanel()
        {
            if (_inspectorPanel == null)
                return;

            ObjectLocator.Instance.RightPanel.Remove(_inspectorPanel.GetComponent<RectTransform>());
            Destroy(_inspectorPanel);
            _inspectorPanel = null;
        }

        private void AddInstructionItem(SceneObject instruction)
        {
            var item = Instantiate(HierarchyItemPrefab);
            item.StepModel = StepModel;
            item.Instruction = instruction;
            item.OnClicked += Item_OnClicked;

            _items.Add(item);
            Container.Add(item.RectTransform);
        }
        private void RemoveInstructionItem(SceneObject instruction)
        {
            var index = _items.FindIndex(element => element.Instruction.ID == instruction.ID);
            if (index == -1)
                return;

            var item = _items[index];

            if (SelectedHierarchyItem == item)
                SelectedHierarchyItem = null;

            Container.Remove(item.RectTransform);
            _items.RemoveAt(index);

            item.OnClicked -= Item_OnClicked;
            Destroy(item.gameObject);
        }

        #region Unity UI Event Handlers
        public void OnNewInstructionButtonClick()
        {
            var panel = Instantiate(NewInstructionPanelPrefab);
            panel.StepModel = StepModel;
            panel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
        }
        #endregion

        #region Event Handlers
        private void Step_OnInstructionAdded(StepModel sender, StepModel.InstructionEventArgs eventArgs)
        {
            AddInstructionItem(eventArgs.Instruction);
        }
        private void Step_OnInstructionRemoved(StepModel sender, StepModel.InstructionEventArgs eventArgs)
        {
            RemoveInstructionItem(eventArgs.Instruction);
        }

        private void Item_OnClicked(HierarchyItem sender, System.EventArgs eventArgs)
        {
            SelectedHierarchyItem = sender;
        }
        #endregion
    }
}
