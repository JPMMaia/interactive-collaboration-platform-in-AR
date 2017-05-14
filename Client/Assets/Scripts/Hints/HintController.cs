using System;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class HintController : Controller
    {
        public HintPanelItemView HintPanelItemViewPrefab;
        public TextHint3DView TextHint3DViewPrefab;
        public ImageHint3DView ImageHint3DViewPrefab;

        public HintModel HintModel { get; set; }
        public RectTransform HintPanelItemViewsContainer { get; set; }

        private HintPanelItemView _hintPanelItemView;
        private Entity _hint3DView;

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

            // Instantiate hint 3D view:
            if (HintModel.Type == HintType.Text)
            {
                var hint3DView = Instantiate(TextHint3DViewPrefab, Application.View.SceneRoot.transform);
                hint3DView.Text = HintModel.Name;
                _hint3DView = hint3DView;
            }
            else if (HintModel.Type == HintType.Image)
            {
                var hint3DView = Instantiate(ImageHint3DViewPrefab, Application.View.SceneRoot.transform);
                var imageHintModel = (ImageHintModel) HintModel;
                hint3DView.Image = Application.View.ImageHintTextures.GetTexture(imageHintModel.ImageHintType);
                _hint3DView = hint3DView;
            }
        }

        public void OnDestroy()
        {
            if(_hint3DView)
                Destroy(_hint3DView.gameObject);

            
            if(_hintPanelItemView)
                Destroy(_hintPanelItemView.gameObject);
        }

        private void _hintPanelItemView_OnNameChanged(object sender, HintPanelItemView.NameEventArgs e)
        {
            HintModel.Name = e.Name;

            if (HintModel.Type == HintType.Text)
            {
                var hint3DView = (TextHint3DView) _hint3DView;
                hint3DView.Text = e.Name;
            }
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
