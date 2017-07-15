using System;

namespace CollaborationEngine.Events
{
    public delegate void IDEventDelegate(object sender, IDEventArgs eventArgs);

    public class IDEventArgs : EventArgs
    {
        public uint ID { get; private set; }

        public IDEventArgs(uint id)
        {
            ID = id;
        }
    }
}
