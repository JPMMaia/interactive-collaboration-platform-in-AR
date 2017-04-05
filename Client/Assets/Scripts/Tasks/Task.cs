using System;
using System.Collections.Generic;

namespace CollaborationEngine.Tasks
{
    public class Task
    {
        public Task(String name)
        {
            Name = name;
        }

        public String Name { get; private set; }

        private List<Step> _steps = new List<Step>();
    }
}
