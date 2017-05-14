using System;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class HintController : Controller
    {
        public HintPanelItemView HintPanelItemViewPrefab;

        public HintModel HintModel { get; set; }
        public RectTransform HintPanelItemViewsContainer { get; set; }

        private HintPanelItemView _hintPanelItemView;

        public void Start()
        {
            // Instantiate hint panel item view:
            _hintPanelItemView = Instantiate(HintPanelItemViewPrefab, HintPanelItemViewsContainer);

            // Set properties:
            _hintPanelItemView.Name = HintModel.Name;

            if (HintModel.Type == HintType.Text)
            {
                _hintPanelItemView.Icon = Application.View.Icons.TextIcon;
            }
            else if (HintModel.Type == HintType.Image)
            {
                var imageHintModel = (ImageHintModel) HintModel;
                _hintPanelItemView.Icon = Application.View.ImageHintTextures.GetTexture(imageHintModel.ImageHintType);
            }

            // Subscribe to events:
            _hintPanelItemView.OnNameChanged += _hintPanelItemView_OnNameChanged;
            _hintPanelItemView.OnDuplicateClicked += _hintPanelItemView_OnDuplicateClicked;
            _hintPanelItemView.OnDeleteClicked += _hintPanelItemView_OnDeleteClicked;

            // TODO Instantiate 3D hint:

        }
        public void OnDestroy()
        {
            // Destroy hint panel item view:
            if(_hintPanelItemView)
                Destroy(_hintPanelItemView.gameObject);
        }

        private void _hintPanelItemView_OnNameChanged(object sender, HintPanelItemView.NameEventArgs e)
        {
            HintModel.Name = e.Name;
        }
        private void _hintPanelItemView_OnDuplicateClicked(object sender, EventArgs e)
        {
            var parentTaskModel = Application.Model.Tasks.Get(HintModel.TaskID);
            var parentStepModel = parentTaskModel.GetStep(HintModel.StepID);

            parentStepModel.DuplicateHint(HintModel.ID);
        }
        private void _hintPanelItemView_OnDeleteClicked(object sender, EventArgs e)
        {
            var parentTaskModel = Application.Model.Tasks.Get(HintModel.TaskID);
            var parentStepModel = parentTaskModel.GetStep(HintModel.StepID);

            parentStepModel.DeleteHint(HintModel.ID);
        }
    }
}
