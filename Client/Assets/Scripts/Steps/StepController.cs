using CollaborationEngine.Base;

namespace CollaborationEngine.Steps
{
    public class StepController : Controller
    {
        public StepView StepView;

        public StepModel StepModel { get; set; }
        public uint StepOrder
        {
            get { return StepView.StepOrder; }
            set { StepView.StepOrder = value; }
        }

        public void Start()
        {
            StepView.StepID = StepModel.ID;
            StepView.StepDescription = StepModel.Name.ToUpper();
        }

        public void OnAddButtonClick()
        {
            // TODO Span hint window
        }
    }
}
