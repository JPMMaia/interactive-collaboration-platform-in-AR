using System;
using CollaborationEngine.Base;
using CollaborationEngine.Cameras;
using CollaborationEngine.RuntimeGizmo.Objects;

namespace CollaborationEngine.Panels
{
    public class TransformPanelController : Controller
    {
        public TransformPanelView TransformPanelViewPrefab;

        private TransformPanelView _view;

        public void Start()
        {
            // Instantiate:
            _view = Instantiate(TransformPanelViewPrefab, Application.View.MainCanvas.transform);

            _view.OnTranslateClicked += _view_OnTranslateClicked;
            _view.OnRotateClicked += _view_OnRotateClicked;
            _view.OnScaleClicked += _view_OnScaleClicked;
        }
        public void OnDestroy()
        {
            Destroy(_view.gameObject);
        }

        private void _view_OnTranslateClicked(object sender, EventArgs e)
        {
            foreach (var gizmo in TransformGizmoManager.Instance.TransformGizmos)
                gizmo.type = TransformType.Move;
        }
        private void _view_OnRotateClicked(object sender, EventArgs e)
        {
            foreach (var gizmo in TransformGizmoManager.Instance.TransformGizmos)
                gizmo.type = TransformType.Rotate;
        }
        private void _view_OnScaleClicked(object sender, EventArgs e)
        {
            foreach (var gizmo in TransformGizmoManager.Instance.TransformGizmos)
                gizmo.type = TransformType.Scale;
        }
    }
}
