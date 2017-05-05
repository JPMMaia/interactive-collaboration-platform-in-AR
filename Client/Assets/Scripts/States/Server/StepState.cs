using CollaborationEngine.Feedback;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI.Console;
using CollaborationEngine.UI.Steps;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class StepState : IApplicationState
    {
        #region Members
        private readonly ServerCollaborationState _serverState;
        private readonly Task _task;
        private StepsPanel _stepPanel;
        private MentorFeedbackModule _mentorFeedbackModule;
        #endregion

        public StepState(ServerCollaborationState serverState, Task task)
        {
            _serverState = serverState;
            _task = task;
        }

        public void Initialize()
        {
            Debug.Log("Initialize StepState");

            {
                _stepPanel = Object.Instantiate(ObjectLocator.Instance.ServerStepsPanelPrefab);
                _stepPanel.Task = _task;
                ObjectLocator.Instance.LeftPanel.Add(_stepPanel.GetComponent<RectTransform>());
            }

            {
                var consolePanel = Object.Instantiate(ObjectLocator.Instance.ConsoleController);
                ObjectLocator.Instance.ConsoleController = consolePanel;
                ObjectLocator.Instance.LeftPanel.Add(consolePanel.GetComponent<RectTransform>());
            }

            _mentorFeedbackModule = new MentorFeedbackModule();
            _mentorFeedbackModule.OnHelpWanted += MentorFeedbackModule_OnHelpWanted;
            _mentorFeedbackModule.OnStepCompleted += MentorFeedbackModule_OnStepCompleted;
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown StepState");

            _mentorFeedbackModule.OnStepCompleted -= MentorFeedbackModule_OnStepCompleted;
            _mentorFeedbackModule.OnHelpWanted -= MentorFeedbackModule_OnHelpWanted;
            _mentorFeedbackModule = null;

            if (_stepPanel)
            {
                Object.Destroy(_stepPanel.gameObject);
                _stepPanel = null;
            }
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }
        public void LateUpdate()
        {
            foreach (var step in _task.Steps)
            {
                foreach (var instruction in step.Instructions)
                    instruction.PerformNetworkSynch();
            }
        }

        private void MentorFeedbackModule_OnHelpWanted(object sender, MentorFeedbackModule.FeedbackEventArgs e)
        {
            Object.FindObjectOfType<ConsoleController>().AddText("[" + e.StepName + "] Apprentice is requesting more instructions.");
        }
        private void MentorFeedbackModule_OnStepCompleted(object sender, MentorFeedbackModule.FeedbackEventArgs e)
        {
            Object.FindObjectOfType<ConsoleController>().AddText("[" + e.StepName + "] Apprentice completed step.");
        }
    }
}
