using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CollaborationEngine.Tasks
{
    public class Task
    {
        public delegate void TaskEventDelegate(Task sender, EventArgs eventArgs);

        public event TaskEventDelegate OnNameChanged;

        public static String GenerateName()
        {
            return "Undefined " + _count;
        }

        public Task(String name)
        {
            Name = name;
            ++_count;
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;

                if(OnNameChanged != null)
                    OnNameChanged(this, EventArgs.Empty);
            }
        }

        private static uint _count;
        private List<Step> _steps = new List<Step>();
        private string _name;
    }
}
