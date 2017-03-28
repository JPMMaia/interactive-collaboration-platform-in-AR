using System;

namespace CollaborationEngine.Objects.Collision
{
    public class InputCollider : AbstractCollider
    {
        public void Press()
        {
            NotifyPressed(this, EventArgs.Empty);
        }

        public void OnMouseDown()
        {
            NotifyMouseDown(this, EventArgs.Empty);
        }
        public void OnMouseUpAsButton()
        {
            NotifyPressed(this, EventArgs.Empty);
        }
        public void OnMouseDrag()
        {
            NotifyDragged(this, EventArgs.Empty);
        }
    }
}