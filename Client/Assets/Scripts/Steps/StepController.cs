using CollaborationEngine.Base;
using CollaborationEngine.Hints;

namespace CollaborationEngine.Steps
{
    public class StepController : Controller
    {
        public StepView StepView;
        public NewHintWindowController NewHintWindowControllerPrefab;

        public StepModel StepModel { get; set; }
        public uint StepOrder
        {
            get { return StepView.StepOrder; }
            set { StepView.StepOrder = value; }
        }

        public void Start()
        {
            StepView.StepID = StepModel.ID;

            if(StepModel.Name != null)
                StepView.StepDescription = StepModel.Name.ToUpper();
        }

        public void OnAddButtonClick()
        {
            // Span hint window:
            Instantiate(NewHintWindowControllerPrefab, Application.View.MainCanvas.transform);
        }
    }
}
