using System;

namespace CollaborationEngine.Objects.Collision
{
    public class InputCollider : AbstractCollider
    {
        public void Press()
        {
            Press(this, EventArgs.Empty);
        }

        public void OnMouseUpAsButton()
        {
            Press(this, EventArgs.Empty);
        }
    }
}