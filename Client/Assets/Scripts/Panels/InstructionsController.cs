using System;
using System.Collections.Generic;
using System.Linq;
using CollaborationEngine.Base;
using CollaborationEngine.Cameras;
using CollaborationEngine.Network;
using CollaborationEngine.Steps;
using CollaborationEngine.Tasks;
using UnityEngine.UI;

namespace CollaborationEngine.Panels
{
    public class InstructionsController : Controller
    {
        #region Unity Editor
        public StepController StepControllerPrefab;
        public InstructionsView InstructionsView;
        public InputField AddStepInputField;
        public Text ShowingStepMessageText;
        #endregion

        #region Properties
        public TaskModel TaskModel { get; set; }
        public uint ShowingStepID
        {
            get { return _showingStepID; }
            set
            {
                if (_stepControllers.ContainsKey(_showingStepID))
                {
                    // Hide step:
                    _stepControllers[_showingStepID].Showing = false;
                }

                _showingStepID = value;

                if (_stepControllers.ContainsKey(_showingStepID))
                {
                    var stepController = _stepControllers[_showingStepID];

                    // Enable radio button:
                    stepController.Showing = true;

                    // Change apprentice box message:
                    ShowingStepMessageText.text = String.Format("Showing Step {0}.", _stepControllers[value].StepOrder);

                    SendPresentStepNetworkMessage();
                }
            }
        }
        public MentorNetworkManager NetworkManager { get; set; }
        public CameraManager CameraManager { get; set; }
        #endregion

        #region Members
        private readonly Dictionary<uint, StepController> _stepControllers = new Dictionary<uint, StepController>();
        private uint _showingStepID;
        #endregion

        public void Start()
        {
            NetworkManager.OnPlayerConnected += NetworkManager_OnPlayerConnected;

            TaskModel.OnStepCreated += TaskModel_OnStepCreated;
            TaskModel.OnStepDuplicated += TaskModel_OnStepDuplicated;
            TaskModel.OnStepDeleted += TaskModel_OnStepDeleted;

            // Create step controllers:
            foreach (var stepModel in TaskModel.Steps)
                CreateStepController(stepModel.Value);

            if (_stepControllers.Count > 0)
                ShowingStepID = _stepControllers.First().Key;
        }

        private void CreateStepController(StepModel stepModel)
        {
            // Ignore if step controller already exists:
            if (_stepControllers.ContainsKey(stepModel.ID))
                return;

            // Instantiate:
            var stepController = Instantiate(StepControllerPrefab);

            // Set properties:
            stepController.StepModel = stepModel;
            stepController.CameraManager = CameraManager;
            stepController.StepOrder = (uint)_stepControllers.Count + 1;

            // Subscribe to events:
            stepController.OnShowClicked += StepController_OnShowClicked;
            stepController.OnDeleteClicked += StepController_OnDeleteClicked;

            // Add to instructions view:
            InstructionsView.AddToContainer(stepController.transform);

            // Add to list:
            _stepControllers.Add(stepModel.ID, stepController);

            // If not showing any step, show this one:
            if (!_stepControllers.ContainsKey(ShowingStepID))
                ShowingStepID = stepModel.ID;
        }
        private void DeleteStepController(uint stepID)
        {
            // GetStep step controller:
            StepController stepController;
            if (!_stepControllers.TryGetValue(stepID, out stepController))
                return;

            // Remove step from list:
            _stepControllers.Remove(stepID);

            // If showing, switch to another:
            if (stepController.Showing)
            {
                if (_stepControllers.Count > 1)
                    ShowingStepID = _stepControllers.First().Key;
            }

            // Remove from instructions view:
            InstructionsView.RemoveFromContainer(stepController.transform);

            // Destroy:
            Destroy(stepController.gameObject);
        }

        public void OnAddStepEndEdit()
        {
            if (AddStepInputField.text.Length == 0)
                return;

            // CreateHint step model:
            var stepModel = TaskModel.CreateStep();
            stepModel.Name = AddStepInputField.text;

            // Reset input field:
            AddStepInputField.text = String.Empty;
        }

        private void SendPresentStepNetworkMessage()
        {
            var stepController = _stepControllers[_showingStepID];
            NetworkManager.client.Send(NetworkHandles.PresentStep, new StepModelNetworkMessage(stepController.StepModel));
        }

        #region Event Handlers
        private void NetworkManager_OnPlayerConnected(object sender, EventArgs e)
        {
            if (NetworkManager.IsAppreticeConnected)
            {
                var stepController = _stepControllers[_showingStepID];
                NetworkManager.client.Send(NetworkHandles.Initialize, new StepModelNetworkMessage(stepController.StepModel));
            }
        }
        private void TaskModel_OnStepCreated(TaskModel sender, StepEventArgs eventArgs)
        {
            CreateStepController(eventArgs.StepModel);
        }
        private void TaskModel_OnStepDuplicated(TaskModel sender, StepEventArgs eventArgs)
        {
            CreateStepController(eventArgs.StepModel);
        }
        private void TaskModel_OnStepDeleted(TaskModel sender, StepEventArgs eventArgs)
        {
            DeleteStepController(eventArgs.StepModel.ID);
        }
        private void StepController_OnShowClicked(object sender, StepView.ShowEventArgs e)
        {
            ShowingStepID = e.StepID;
        }
        private void StepController_OnDeleteClicked(object sender, Events.IDEventArgs e)
        {
            // Delete step model:
            TaskModel.DeleteStep(e.ID);
        }
        #endregion
    }
}
