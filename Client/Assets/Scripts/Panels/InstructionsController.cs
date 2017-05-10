using System.Collections.Generic;
using CollaborationEngine.Base;
using CollaborationEngine.Steps;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.Panels
{
    public class InstructionsController : Controller
    {
        #region Unity Editor
        public StepView StepViewPrefab;
        public InstructionsView InstructionsView;
        #endregion

        #region Properties
        public TaskModel TaskModel { get; set; }
        #endregion

        #region Members
        private readonly Dictionary<uint, StepView> _stepViews = new Dictionary<uint, StepView>();
        #endregion

        public void Start()
        {
            TaskModel.OnStepCreated += TaskModel_OnStepCreated;
            TaskModel.OnStepDuplicated += TaskModel_OnStepDuplicated;
            TaskModel.OnStepDeleted += TaskModel_OnStepDeleted;

            // CreateStep step views:
            foreach (var stepModel in TaskModel.Steps)
                CreateTaskView(stepModel.Value);
        }

        private StepView CreateTaskView(StepModel stepModel)
        {
            // Ignore if step view already exists:
            if (_stepViews.ContainsKey(stepModel.ID))
                return _stepViews[stepModel.ID];

            // Instantiate:
            var stepView = Instantiate(StepViewPrefab);

            // TODO Set properties:

            // TODO Subscribe to events:

            // Add to instructions view:
            InstructionsView.AddToContainer(stepView.transform);

            // Add to list:
            _stepViews.Add(stepModel.ID, stepView);

            return stepView;
        }
        private void DeleteTaskView(uint stepID)
        {
            // GetStep step view:
            StepView stepView;
            if (!_stepViews.TryGetValue(stepID, out stepView))
                return;

            // Remove step from list:
            _stepViews.Remove(stepID);

            // Remove from instructions view:
            InstructionsView.RemoveFromContainer(stepView.transform);

            // Destroy:
            Destroy(stepView.gameObject);
        }

        #region Event Handlers
        private void TaskModel_OnStepCreated(TaskModel sender, StepEventArgs eventArgs)
        {
            CreateTaskView(eventArgs.StepModel);
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
