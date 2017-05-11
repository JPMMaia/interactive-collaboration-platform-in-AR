using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
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
        #endregion

        #region Properties
        public TaskModel TaskModel { get; set; }
        #endregion

        #region Members
        private readonly Dictionary<uint, StepController> _stepControllers = new Dictionary<uint, StepController>();
        #endregion

        public void Start()
        {
            TaskModel.OnStepCreated += TaskModel_OnStepCreated;
            TaskModel.OnStepDuplicated += TaskModel_OnStepDuplicated;
            TaskModel.OnStepDeleted += TaskModel_OnStepDeleted;

            // CreateStep step views:
            foreach (var stepModel in TaskModel.Steps)
                CreateStepController(stepModel.Value);
        }

        private StepController CreateStepController(StepModel stepModel)
        {
            // Ignore if step controller already exists:
            if (_stepControllers.ContainsKey(stepModel.ID))
                return _stepControllers[stepModel.ID];

            // Instantiate:
            var stepController = Instantiate(StepControllerPrefab);

            // Set properties:
            stepController.StepModel = stepModel;
            stepController.StepOrder = (uint)_stepControllers.Count + 1;

            // Add to instructions view:
            InstructionsView.AddToContainer(stepController.transform);

            // Add to list:
            _stepControllers.Add(stepModel.ID, stepController);

            return stepController;
        }
        private void DeleteStepController(uint stepID)
        {
            // GetStep step controller:
            StepController stepView;
            if (!_stepControllers.TryGetValue(stepID, out stepView))
                return;

            // Remove step from list:
            _stepControllers.Remove(stepID);

            // Remove from instructions view:
            InstructionsView.RemoveFromContainer(stepView.transform);

            // Destroy:
            Destroy(stepView.gameObject);
        }

        public void OnAddStepEndEdit()
        {
            // CreateHint step model:
            var stepModel = TaskModel.CreateStep();
            stepModel.Name = AddStepInputField.text;

            // Reset input field:
            AddStepInputField.text = String.Empty;
        }

        #region Event Handlers
        private void TaskModel_OnStepCreated(TaskModel sender, StepEventArgs eventArgs)
        {
            CreateStepController(eventArgs.StepModel);
        }
        private void TaskModel_OnStepDuplicated(TaskModel sender, StepEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
        private void TaskModel_OnStepDeleted(TaskModel sender, StepEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
