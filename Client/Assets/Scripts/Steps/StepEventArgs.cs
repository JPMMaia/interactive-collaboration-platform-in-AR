using System;

namespace CollaborationEngine.Steps
{
    public class StepEventArgs : EventArgs
    {
        public StepModel StepModel { get; private set; }

        public StepEventArgs(StepModel stepModel)
        {
            StepModel = stepModel;
        }
    }
}
