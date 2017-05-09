using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Panels
{
    public class EditorController : Controller
    {
        public delegate void EventDelegate(EditorController sender, EventArgs eventArgs);

        public event EventDelegate OnGoBack;

        public EditorView EditorViewPrefab;

        private EditorView _editorView;

        public void Start()
        {
            // Create editor view:
            _editorView = Instantiate(EditorViewPrefab);

            // Add to canvas:
            _editorView.transform.SetParent(Application.View.MainCanvas.transform, false);

            // Subscribe to events:
            _editorView.OnGoBack += EditorView_OnGoBack;
        }
        public void OnDestroy()
        {
            if(_editorView)
                Destroy(_editorView.gameObject);
        }

        private void EditorView_OnGoBack(object sender, EventArgs e)
        {
            if (OnGoBack != null)
                OnGoBack(this, e);
        }
    }
}
