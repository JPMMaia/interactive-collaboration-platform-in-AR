using System;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Panels
{
    public class StartMentorController : Entity
    {
        public StartMentorView StartMentorViewPrefab;

        public void Start()
        {
            // Create start mentor view:
            var startMentorView = Instantiate(StartMentorViewPrefab);

            // Add to canvas:
            startMentorView.transform.SetParent(Application.View.MainCanvas.transform, false);

            // Subscribe to events:
            startMentorView.OnTaskSelected += StartMentorView_OnTaskSelected;
        }

        private void StartMentorView_OnTaskSelected(object sender, Events.IDEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
