using System;
using CollaborationEngine.Objects;
using CollaborationEngine.States;
using CollaborationEngine.States.Server;
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
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.Arrow);
        }

        public void OnRotateClockwiseClick()
        {
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.RotateClockwise);
        }

        public void OnRotateCounterclockwiseClick()
        {
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.RotateCounterclockwise);
        }

        public void OnWrenchClick()
        {
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.Wrench);
        }

        public void OnAxeClick()
        {
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.Axe);
        }

        public void OnScrewerClick()
        {
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.Screwer);
        }

        public void OnHammerClick()
        {
            State.CurrentState = new IndicationButtonClickedState(State, IndicationType.Hammer);
        }

        public ServerCollaborationState State { get; set; }

        private static ServerUIController _instance;
    }
}
