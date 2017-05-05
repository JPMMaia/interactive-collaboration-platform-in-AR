using CollaborationEngine.Feedback;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI.Feedback
{
    public class FeedbackPanel : MonoBehaviour
    {
        public ApprenticeFeedbackModule FeedbackModule { get; set; }
        public Step CurrentStep { get; set; }

        #region Unity Event Handlers
        public void OnHelpButtonClicked()
        {
            FeedbackModule.HelpWanted(CurrentStep.Name);
        }
        public void OnStepCompletedButtonClicked()
        {
            FeedbackModule.StepCompleted(CurrentStep.Name);
        }
        #endregion
    }
}
