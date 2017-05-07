﻿using System;
using System.Collections.Generic;
using CollaborationEngine.Base;

namespace CollaborationEngine.Tasks
{
    public class TasksModel : Entity
    {
        #region Properties
        public IEnumerable<TaskModel> Tasks
        {
            get { return _tasks; }
        }
        #endregion

        private readonly List<TaskModel> _tasks = new List<TaskModel>();

        public TaskModel Create()
        {
            throw new NotImplementedException();
        }
        public TaskModel Duplicate(uint taskID)
        {
            throw new NotImplementedException();
        }
        public void Delete(uint taskID)
        {
            _tasks.RemoveAll(e => e.ID == taskID);
        }
        public TaskModel Get(uint taskID)
        {
            return _tasks.Find(e => e.ID == taskID);
        }
    }
}