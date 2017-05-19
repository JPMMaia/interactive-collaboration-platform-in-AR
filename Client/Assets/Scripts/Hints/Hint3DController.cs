using CollaborationEngine.Base;

namespace CollaborationEngine.Hints
{
    public class Hint3DController : Controller
    {
        public TextHint3DView TextHint3DViewPrefab;
        public ImageHint3DView ImageHint3DViewPrefab;

        public HintModel HintModel { get; set; }
        public Hint3DView Hint3DView
        {
            get
            {
                return _hint3DView;
            }
        }

        private Hint3DView _hint3DView;

        public void Start()
        {
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

            _hint3DView.LocalPosition = HintModel.LocalPosition;
            _hint3DView.LocalRotation = HintModel.LocalRotation;
            _hint3DView.LocalScale = HintModel.LocalScale;
            _hint3DView.Showing = true;
        }
        public void OnDestroy()
        {
            if (_hint3DView)
                Destroy(_hint3DView.gameObject);
        }
    }
}
