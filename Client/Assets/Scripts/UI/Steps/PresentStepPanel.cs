using CollaborationEngine.Network;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Steps
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PresentStepPanel : MonoBehaviour
    {
        #region Unity Editor
        public Text ButtonText;
        #endregion

        #region Properties
        public StepsPanel StepsPanel { get; set; }
        public StepModel StepModel { get; set; }
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();

                return _canvasGroup;
            }
        }

        public bool PresentingStep
        {
            get { return _presentingStep; }
            set
            {
                _presentingStep = value;
                UpdateAppearance();
            }
        }

        #endregion

        #region Members
        private CanvasGroup _canvasGroup;
        private bool _presentingStep;

        #endregion

        public void Awake()
        {
            var networkController = NetworkController.Instance;
            networkController.OnPlayerConnected += NetworkController_OnPlayerConnected;
            networkController.OnPlayerDisconnected += NetworkController_OnPlayerDisconnected;

            UpdateAppearance();
        }
        public void OnDestroy()
        {
            var networkController = NetworkController.Instance;
            if (networkController != null)
            {
                networkController.OnPlayerDisconnected -= NetworkController_OnPlayerDisconnected;
                networkController.OnPlayerConnected -= NetworkController_OnPlayerConnected;
            }
        }

        public void OnButtonClick()
        {
            if (!PresentingStep)
            {
                PresentingStep = true;

                // Send data:
                var networkClient = NetworkManager.singleton.client;
                if (!networkClient.Send(NetworkHandles.PresentStep, new GenericNetworkMessage<StepModel>(StepModel)))
                {
                    PresentingStep = false;
                }
            }
            else
            {
                PresentingStep = false;

                // Send data:
                var networkClient = NetworkManager.singleton.client;
                if (!networkClient.Send(NetworkHandles.StopPresentStep, new IDMessage {ID = StepModel.ID}))
                {
                    PresentingStep = true;
                }
            }
        }

        private void UpdateAppearance()
        {
            if (PresentingStep)
            {
                ButtonText.text = "Stop Presenting";
                StepsPanel.GetComponent<CanvasGroup>().interactable = false;
            }
            else if (NetworkController.Instance.IsAppreticeConnected)
            {
                CanvasGroup.interactable = true;
                ButtonText.text = "Present StepModel";
                if(StepsPanel != null)
                    StepsPanel.GetComponent<CanvasGroup>().interactable = true;
            }
            else
            {
                CanvasGroup.interactable = false;
                ButtonText.text = "Waiting for apprentice";
            }
        }

        #region Event Handlers
        private void NetworkController_OnPlayerConnected(object sender, System.EventArgs e)
        {
            UpdateAppearance();
        }
        private void NetworkController_OnPlayerDisconnected(object sender, System.EventArgs e)
        {
            UpdateAppearance();
        }
        #endregion
    }
}