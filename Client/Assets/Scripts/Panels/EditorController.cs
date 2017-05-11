﻿using System;
using CollaborationEngine.Base;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.Panels
{
    public class EditorController : Controller
    {
        #region Events
        public delegate void EventDelegate(EditorController sender, EventArgs eventArgs);

        public event EventDelegate OnGoBack;
        #endregion

        #region Unity Editor
        public EditorView EditorView;
        public InstructionsController InstructionsController;
        #endregion

        #region Properties
        public uint TaskID { get; set; }
        private TasksModel TasksModel
        {
            get { return Application.Model.Tasks; }
        }
        #endregion

        #region Members
        private TaskModel _task;
        #endregion

        public void Start()
        {
            // Subscribe to events:
            EditorView.OnGoBack += EditorView_OnGoBack;

            // Cache task:
            _task = TasksModel.Get(TaskID);
            InstructionsController.TaskModel = _task;

            // TODO Load steps and hints:

            // TODO If the task hasn't got any steps, add a default one:
            _task.CreateStep();
        }
        public void OnDestroy()
        {
            // TODO Save and then destroy steps and hints:

            if (EditorView)
                Destroy(EditorView.gameObject);
        }

        private void EditorView_OnGoBack(object sender, EventArgs e)
        {
            if (OnGoBack != null)
                OnGoBack(this, e);
        }
    }
}