using System;
using CollaborationEngine.Objects;
using CollaborationEngine.States;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class ServerUIController : MonoBehaviour
    {
        public static ServerUIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ServerUIController>();
                    if (_instance == null)
                        throw new Exception("There should be one server UI controller on scene.");
                }
                    
                return _instance;
            }
        }

        public void OnArrowClick()
        {
            State.SelectedIndicationType = IndicationType.Arrow;
        }

        public void OnRotateClockwiseClick()
        {
            State.SelectedIndicationType = IndicationType.RotateClockwise;
        }

        public void OnRotateCounterclockwiseClick()
        {
            State.SelectedIndicationType = IndicationType.RotateCounterclockwise;
        }

        public void OnWrenchClick()
        {
            State.SelectedIndicationType = IndicationType.Wrench;
        }

        public void OnAxeClick()
        {
            State.SelectedIndicationType = IndicationType.Axe;
        }

        public void OnScrewerClick()
        {
            State.SelectedIndicationType = IndicationType.Screwer;
        }

        public void OnHammerClick()
        {
            State.SelectedIndicationType = IndicationType.Hammer;
        }

        public ServerCollaborationState State { get; set; }

        private static ServerUIController _instance;
    }
}
