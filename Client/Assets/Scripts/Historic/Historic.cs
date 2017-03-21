using System.Collections.Generic;
using CollaborationEngine.States;

namespace CollaborationEngine.Historic
{
    public class History
    {
        public Stack<Action> Actions { get; }

        public History()
        {
            Actions = new Stack<Action>();
        }

        public void PushAction(IApplicationState initialState, IApplicationState finalState)
        {
            Actions.Push(new Action(initialState, finalState));
        }

        public Action PopAction()
        {
            return Actions.Pop();
        }

        public Action PeekAction()
        {
            return Actions.Peek();
        }

        public bool IsEmpty()
        {
            return Actions.Count == 0;
        }
    }
}
