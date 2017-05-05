using CollaborationEngine.Feedback;
using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI.Feedback;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.States.Client
{
    public class StepState : IApplicationState
    {
        #region Members
        private readonly ClientCollaborationState _clientState;
        private readonly Step _step;
        private FeedbackPanel _feedbackPanel;
        private ApprenticeFeedbackModule _apprenticeFeedbackModule;
        #endregion

        public StepState(ClientCollaborationState clientState, Step step)
        {
            _clientState = clientState;
            _step = step;
        }

        public void Initialize()
        {
            var networkManager = NetworkManager.singleton.client;
            networkManager.RegisterHandler(NetworkHandles.StopPresentStep, OnStopStep);
            networkManager.RegisterHandler(NetworkHandles.AddInstruction, OnAddInstruction);
            networkManager.RegisterHandler(NetworkHandles.RemoveInstruction, OnRemoveInstruction);
            networkManager.RegisterHandler(NetworkHandles.UpdateInstruction, OnUpdateInstruction);

            // Instantiate instructions:
            foreach (var instruction in _step.Instructions)
                instruction.Instantiate(ObjectLocator.Instance.SceneRoot.transform);

            _apprenticeFeedbackModule = new ApprenticeFeedbackModule();
            _feedbackPanel = Object.Instantiate(ObjectLocator.Instance.FeedbackPanelPrefab);
            _feedbackPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
            _feedbackPanel.CurrentStep = _step;
            _feedbackPanel.FeedbackModule = _apprenticeFeedbackModule;

            ObjectLocator.Instance.HintText.SetText("Follow the mentor's instructions");
        }
        public void Shutdown()
        {
            if (_feedbackPanel != null)
            {
                Object.Destroy(_feedbackPanel);
                _feedbackPanel = null;
            }

            _apprenticeFeedbackModule = null;

            foreach (var instruction in _step.Instructions)
                instruction.Destroy();

            var networkManager = NetworkManager.singleton.client;
            networkManager.UnregisterHandler(NetworkHandles.UpdateInstruction);
            networkManager.UnregisterHandler(NetworkHandles.RemoveInstruction);
            networkManager.UnregisterHandler(NetworkHandles.AddInstruction);
            networkManager.UnregisterHandler(NetworkHandles.StopPresentStep);
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
        }
        public void LateUpdate()
        {
        }

        private void OnStopStep(NetworkMessage networkMessage)
        {
            _clientState.CurrentState = new WaitForStepState(_clientState);
        }
        private void OnAddInstruction(NetworkMessage networkMessage)
        {
            var instruction = networkMessage.ReadMessage<SceneObject.DataMessage>().Data;

            _step.Instructions.Add(instruction);
            instruction.Instantiate(ObjectLocator.Instance.SceneRoot.transform);
        }
        private void OnRemoveInstruction(NetworkMessage networkMessage)
        {
            var instructionID = networkMessage.ReadMessage<SceneObject.IDMessage>().ID;

            var instructionIndex = _step.Instructions.FindIndex(e => e.ID == instructionID);
            var instruction = _step.Instructions[instructionIndex];
            _step.Instructions.RemoveAt(instructionIndex);

            Object.Destroy(instruction.GameObject);
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {
            var instruction = networkMessage.ReadMessage<SceneObject.DataMessage>().Data;

            var instructionToUpdate = _step.Instructions.Find(e => e.ID == instruction.ID);
            instructionToUpdate.Update(instruction);
        }
    }
}
