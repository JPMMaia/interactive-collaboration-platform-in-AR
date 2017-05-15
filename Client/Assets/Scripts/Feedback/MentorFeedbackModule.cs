using System;
using CollaborationEngine.Network;
using UnityEngine.Networking;

namespace CollaborationEngine.Feedback
{
    public class MentorFeedbackModule
    {
        #region Events
        public class FeedbackEventArgs : EventArgs
        {
            public String StepName { get; set; }
        }
        public event EventHandler<FeedbackEventArgs> OnHelpWanted;
        public event EventHandler<FeedbackEventArgs> OnStepCompleted;
        #endregion

        public MentorFeedbackModule()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.RegisterHandler(NetworkHandles.NeedMoreInstructions, OnHelpWantedCallback);
            networkManager.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompletedCallback);
        }
        ~MentorFeedbackModule()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.UnregisterHandler(NetworkHandles.StepCompleted);
            networkManager.UnregisterHandler(NetworkHandles.NeedMoreInstructions);
        }

        private void OnHelpWantedCallback(NetworkMessage networkMessage)
        {
            if(OnHelpWanted != null)
                OnHelpWanted(this, new FeedbackEventArgs{StepName = networkMessage.ReadMessage<ApprenticeFeedbackModule.StringMessage>().Data });
        }
        private void OnStepCompletedCallback(NetworkMessage networkMessage)
        {
            if (OnStepCompleted != null)
                OnStepCompleted(this, new FeedbackEventArgs { StepName = networkMessage.ReadMessage<ApprenticeFeedbackModule.StringMessage>().Data });
        }
    }
}
