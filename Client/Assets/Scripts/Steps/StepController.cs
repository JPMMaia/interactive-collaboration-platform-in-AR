using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using CollaborationEngine.Events;
using CollaborationEngine.Hints;
using CollaborationEngine.Hints.NewHintWindow;
using UnityEngine;

namespace CollaborationEngine.Steps
{
    public class StepController : Controller
    {
        public event EventHandler<StepView.ShowEventArgs> OnShowClicked;
        public event EventHandler<IDEventArgs> OnDeleteClicked;

        public StepView StepView;
        public RectTransform HintControllersContainer;
        public RectTransform HintPanelItemViewsContainer;
        public NewHintWindowController NewHintWindowControllerPrefab;
        public TextHintModel TextHintModelPrefab;
        public ImageHintModel ImageHintModelPrefab;
        public HintController HintControllerPrefab;

        public StepModel StepModel { get; set; }
        public uint StepOrder
        {
            get { return StepView.StepOrder; }
            set { StepView.StepOrder = value; }
        }
        public bool Showing
        {
            get { return StepView.Showing; }
            set
            {
                StepView.Showing = value;

                foreach (var hint in _hints)
                    hint.Value.Showing = value;
            }
        }

        private readonly Dictionary<uint, HintController> _hints = new Dictionary<uint, HintController>();
        private float _originalHeight;

        public void Start()
        {
            _originalHeight = GetComponent<RectTransform>().rect.height;

            StepView.StepID = StepModel.ID;
            StepView.OnShowClicked += StepView_OnShowClicked;
            StepView.OnDescriptionEndedEdit += StepView_OnDescriptionEndedEdit;

            if (StepModel.Name != null)
                StepView.StepDescription = StepModel.Name.ToUpper();

            StepModel.OnHintCreated += StepModel_OnHintCreated;
            StepModel.OnHintDuplicated += StepModel_OnHintCreated;
            StepModel.OnHintDeleted += StepModel_OnHintDeleted;

            foreach (var hintModel in StepModel.Hints)
            {
                CreateHintController(hintModel.Value);
            }
        }

        private void CreateHintController(HintModel hintModel)
        {
            // Instantiate hint controller:
            var hintController = Instantiate(HintControllerPrefab, HintControllersContainer);
            hintController.HintPanelItemViewsContainer = HintPanelItemViewsContainer;
            hintController.HintModel = hintModel;
            hintController.Showing = Showing;

            // Add to collection:
            _hints.Add(hintModel.ID, hintController);

            UpdatePanelSize();
        }
        private void DeleteHintController(uint hintID)
        {
            // Remove from collection:
            HintController hintController;
            if (!_hints.TryGetValue(hintID, out hintController))
                return;
            _hints.Remove(hintID);

            // Destroy hint controller:
            Destroy(hintController.gameObject);

            UpdatePanelSize();
        }

        public void OnAddButtonClick()
        {
            // Span hint window:
            var newHintWindowController = Instantiate(NewHintWindowControllerPrefab, Application.View.MainCanvas.transform);
            newHintWindowController.OnEndCreate += NewHintWindowController_OnEndCreate;
        }
        public void OnDeleteButtonClick()
        {
            if(OnDeleteClicked != null)
                OnDeleteClicked(this, new IDEventArgs(StepModel.ID));
        }

        private void NewHintWindowController_OnEndCreate(object sender, NewHintWindowController.WindowDataEventArgs eventArgs)
        {
            uint hintID;

            // Create hint model:
            if (eventArgs.HintType == HintType.Text)
            {
                var hintModel = StepModel.CreateHint(TextHintModelPrefab);
                hintModel.Name = eventArgs.Name;
                hintID = hintModel.ID;
            }
            else
            {
                var hintModel = StepModel.CreateHint(ImageHintModelPrefab);
                hintModel.Name = eventArgs.Name;
                hintModel.ImageHintType = eventArgs.ImageHintType;
                hintID = hintModel.ID;
            }

            // Get hint controller:
            var hintController = _hints[hintID];
            if(Showing)
                hintController.Edit();
        }

        private void UpdatePanelSize()
        {
            var rectTransform = GetComponent<RectTransform>();

            var size = _originalHeight + _hints.Count * 40.0f + (_hints.Count - 1) * 10.0f;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }

        private void StepView_OnShowClicked(object sender, StepView.ShowEventArgs e)
        {
            if (OnShowClicked != null)
                OnShowClicked(this, e);
        }
        private void StepView_OnDescriptionEndedEdit(object sender, StepView.EndEditEventArgs e)
        {
            StepModel.Name = e.Text;
        }
        private void StepModel_OnHintCreated(StepModel sender, HintEventArgs eventArgs)
        {
            CreateHintController(eventArgs.HintModel);
        }
        private void StepModel_OnHintDeleted(StepModel sender, HintEventArgs eventArgs)
        {
            DeleteHintController(eventArgs.HintModel.ID);
        }
    }
}
