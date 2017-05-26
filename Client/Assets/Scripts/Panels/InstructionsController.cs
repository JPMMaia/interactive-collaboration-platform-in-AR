using System;
using System.Collections.Generic;
using System.IO;
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

                    {
                        // Log to file:
                        using (var stream = new FileStream(_sessionFilename, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            using (var streamWriter = new StreamWriter(stream))
                            {
                                streamWriter.WriteLine("Step {0} at {1}", _stepControllers[value].StepOrder, DateTime.Now);
                            }
                        }
                    }

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
        private String _sessionFilename;
        #endregion

        public void Start()
        {
            {
                // Create directory if it doesn't exist:
                const string directory = "Sessions/";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Set filename:
                _sessionFilename = directory + String.Format("{0:yyyy-mm-dd_hh-MM-ss}.session", DateTime.Now);

                // Write task ID and name:
                using (var stream = new FileStream(_sessionFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        streamWriter.WriteLine("Task ID: {0}", TaskModel.ID);
                        streamWriter.WriteLine("Task Name: {0}", TaskModel.Name);
                    }
                }
            }

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
            stepController.OnHintEditClicked += StepController_OnHintEditClicked;

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
            var stepModel = TaskModel.DuplicateStep(TaskModel.Steps.Last().Key);
            stepModel.Name = AddStepInputField.text;

            // Reset input field:
            AddStepInputField.text = String.Empty;
        }

        private void SendPresentStepNetworkMessage()
        {
            var stepController = _stepControllers[_showingStepID];
            NetworkManager.client.Send(NetworkHandles.PresentStep, new StepModelNetworkMessage(TaskModel.ImageTargetIndex, stepController.StepOrder, stepController.StepModel));
        }

        #region Event Handlers
        private void NetworkManager_OnPlayerConnected(object sender, EventArgs e)
        {
            if (NetworkManager.IsAppreticeConnected)
            {
                var stepController = _stepControllers[_showingStepID];
                NetworkManager.client.Send(NetworkHandles.Initialize, new StepModelNetworkMessage(TaskModel.ImageTargetIndex, stepController.StepOrder, stepController.StepModel));
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
        private void StepController_OnHintEditClicked(object sender, StepController.StepHintEventArgs e)
        {
            ShowingStepID = e.Sender.StepModel.ID;
        }
        #endregion
    }
}
