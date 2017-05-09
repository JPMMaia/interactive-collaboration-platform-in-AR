using System;

namespace CollaborationEngine.Hints
{
    public class HintEventArgs : EventArgs
    {
            public HintModel HintModel { get; private set; }

            public HintEventArgs(HintModel hintModel)
            {
            HintModel = hintModel;
            }
    }
}
