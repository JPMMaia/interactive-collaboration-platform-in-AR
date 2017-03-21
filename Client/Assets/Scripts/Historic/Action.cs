using CollaborationEngine.States;

namespace CollaborationEngine.Historic
{
    public class Action
    {
        public IApplicationState InitialState { get; set; }
        public IApplicationState FinalState { get; set; }

        public Action(IApplicationState initialState, IApplicationState finalState)
        {
            InitialState = initialState;
            FinalState = finalState;
        }
    }
}
