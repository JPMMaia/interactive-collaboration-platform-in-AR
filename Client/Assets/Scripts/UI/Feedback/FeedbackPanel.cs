using CollaborationEngine.Feedback;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI.Feedback
{
    public class FeedbackPanel : MonoBehaviour
    {
        public ApprenticeFeedbackModule FeedbackModule { get; set; }
        public StepModel CurrentStepModel { get; set; }

        #region Unity Event Handlers
        public void OnHelpButtonClicked()
        {
            FeedbackModule.HelpWanted(CurrentStepModel.Name);
        }
        public void OnStepCompletedButtonClicked()
        {
            FeedbackModule.StepCompleted(CurrentStepModel.Name);
        }
        #endregion
    }
}
