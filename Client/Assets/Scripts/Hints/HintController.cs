using System;
using CollaborationEngine.Base;
using CollaborationEngine.Cameras;
using CollaborationEngine.Hints.NewHintWindow;
using CollaborationEngine.Network;
using CollaborationEngine.Panels;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class HintController : Controller
    {
        #region Events
        public class HintEventArgs : EventArgs
        {
            public HintController HintController { get; private set; }

            public HintEventArgs(HintController hintController)
            {
                HintController = hintController;
            }
        }

        public event EventHandler<HintEventArgs> OnEdit;
        #endregion

        public EditImageHintWindowController EditHintWindowControllerPrefab;
        public HintPanelItemView HintPanelItemViewPrefab;
        public TextHint3DView TextHint3DViewPrefab;
        public ImageHint3DView ImageHint3DViewPrefab;
        public GeometryHint3DView GeometryHint3DViewPrefab;
        public TransformPanelController TransformPanelControllerPrefab;

        public HintModel HintModel { get; set; }
        public RectTransform HintPanelItemViewsContainer { get; set; }
        public bool Showing
        {
            get { return _showing; }
            set
            {
                _showing = value;
                if (_hint3DView)
                    _hint3DView.Showing = value;
            }
        }
        public Vector3 Position
        {
            get { return HintModel.Position; }
            set
            {
                HintModel.Position = value;

                if (_hint3DView)
                    _hint3DView.Position = value;
            }
        }
        public Quaternion Rotation
        {
            get { return HintModel.Rotation; }
            set
            {
                HintModel.Rotation = value;

                if (_hint3DView)
                    _hint3DView.Rotation = value;
            }
        }

        private HintPanelItemView _hintPanelItemView;
        private Hint3DView _hint3DView;
        private bool _showing;
        private bool _edit;
        private TransformPanelController _transformPanelController;

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
                var imageHintModel = (ImageHintModel)HintModel;
                _hintPanelItemView.Icon = Application.View.ImageHintTextures.GetTexture(imageHintModel.ImageHintType);
                _hintPanelItemView.OnIconClicked += _hintPanelItemView_OnIconClicked;
            }
            else if (HintModel.Type == HintType.Geometry)
            {
                _hintPanelItemView.Icon = Application.View.Icons.GeometryIcon;
            }
            else
            {
                throw new NotSupportedException();
            }

            // Subscribe to events:
            _hintPanelItemView.OnNameChanged += _hintPanelItemView_OnNameChanged;
            _hintPanelItemView.OnEditClicked += _hintPanelItemView_OnEditClicked;
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
                var imageHintModel = (ImageHintModel)HintModel;
                hint3DView.Image = Application.View.ImageHintTextures.GetTexture(imageHintModel.ImageHintType);
                _hint3DView = hint3DView;
            }
            else if (HintModel.Type == HintType.Geometry)
            {
                var hint3DView = Instantiate(GeometryHint3DViewPrefab, Application.View.SceneRoot.transform);
                var geometryHintModel = (GeometryHintModel)HintModel;
                hint3DView.Geometry = Application.View.GeometryModels.GetGeometry(geometryHintModel.ModelID);
                _hint3DView = hint3DView;
            }
            else
            {
                throw new NotSupportedException();
            }

            _hint3DView.LocalPosition = HintModel.LocalPosition;
            _hint3DView.LocalRotation = HintModel.LocalRotation;
            _hint3DView.LocalScale = HintModel.LocalScale;
            _hint3DView.Showing = Showing;

            if (_edit)
                _hintPanelItemView.OnEditClick();
        }
        public void OnDestroy()
        {
            if (_hint3DView)
                Destroy(_hint3DView.gameObject);


            if (_hintPanelItemView)
                Destroy(_hintPanelItemView.gameObject);
        }

        public void Edit()
        {
            _edit = true;

            if (_hintPanelItemView)
                _hintPanelItemView.OnEditClick();
        }

        private void _hintPanelItemView_OnNameChanged(object sender, HintPanelItemView.NameEventArgs e)
        {
            HintModel.Name = e.Name;

            if (HintModel.Type == HintType.Text)
            {
                var hint3DView = (TextHint3DView)_hint3DView;
                hint3DView.Text = e.Name;
            }
        }
        private void _hintPanelItemView_OnEditClicked(object sender, HintPanelItemView.EditEventArgs e)
        {
            // Set transform gizmo:
            var transformGizmo = FindObjectOfType<TransformGizmoManager>();
            transformGizmo.Target = e.Editing ? _hint3DView.transform : null;

            if (e.Editing)
            {
                _transformPanelController = Instantiate(TransformPanelControllerPrefab, transform);
                transformGizmo.OnTargetTransformChanged += TransformGizmo_OnTargetTransformChanged;
            }
            else
            {
                transformGizmo.OnTargetTransformChanged -= TransformGizmo_OnTargetTransformChanged;
                if (_transformPanelController)
                    Destroy(_transformPanelController.gameObject);
            }

            if (OnEdit != null)
                OnEdit(this, new HintEventArgs(this));
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
        private void _hintPanelItemView_OnIconClicked(object sender, EventArgs e)
        {
            var editWindow = Instantiate(EditHintWindowControllerPrefab, Application.View.MainCanvas.transform);
            editWindow.SelectedImageHintType = (uint)((ImageHintModel)HintModel).ImageHintType;

            editWindow.OnEndCreate += EditWindow_OnEndCreate;
        }
        private void TransformGizmo_OnTargetTransformChanged(object sender, TransformGizmoManager.TransformEventArgs e)
        {
            // Update model:
            HintModel.LocalPosition = e.Transform.localPosition;
            HintModel.LocalRotation = e.Transform.localRotation;
            HintModel.LocalScale = e.Transform.localScale;

            // Send network message:
            var networkManager = MentorNetworkManager.Instance;
            if (networkManager.IsAppreticeConnected)
            {
                var message = new TransformNetworkMessage(HintModel.ID, e.Transform.localPosition,
                    e.Transform.localRotation, e.Transform.localScale);
                networkManager.client.Send(NetworkHandles.UpdateHintTransform, message);
            }
        }
        private void EditWindow_OnEndCreate(object sender, EditImageHintWindowController.WindowDataEventArgs e)
        {
            var imageHintModel = (ImageHintModel)HintModel;
            imageHintModel.ImageHintType = (ImageHintType)e.ImageHintType;

            var texture = Application.View.ImageHintTextures.GetTexture(imageHintModel.ImageHintType);
            _hintPanelItemView.Icon = texture;
            ((ImageHint3DView)_hint3DView).Image = texture;
        }
    }
}
