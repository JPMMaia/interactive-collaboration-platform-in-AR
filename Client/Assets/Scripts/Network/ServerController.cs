using CollaborationEngine.Base;
using CollaborationEngine.Steps;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        private Application Application
        {
            get { return FindObjectOfType<Application>(); }
        }
        private StepModel CurrentStepModel
        {
            get
            {
                return _currentStepModel;
            }
            set
            {
                if(_currentStepModel)
                    Destroy(_currentStepModel.gameObject);

                _currentStepModel = value;

                if(_currentStepModel)
                    _currentStepModel.transform.SetParent(transform, false);
            }
        }

        private StepModel _currentStepModel;

        public void Awake()
        {
            NetworkServer.RegisterHandler(NetworkHandles.Initialize, OnInitialize);
            NetworkServer.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
            NetworkServer.RegisterHandler(NetworkHandles.UpdateHintTransform, OnUpdateInstruction);
            NetworkServer.RegisterHandler(NetworkHandles.NeedMoreInstructions, OnNeedMoreInstructions);
            NetworkServer.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompleted);
        }

        private void OnInitialize(NetworkMessage networkMessage)
        {
            var currentStepParentTask = Application.Model.Tasks.Get(CurrentStepModel.TaskID);
            var message = new StepModelNetworkMessage(currentStepParentTask.ImageTargetIndex, CurrentStepModel);
            NetworkServer.SendToAll(NetworkHandles.Initialize, message);
        }
        private void OnPresentStep(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<StepModelNetworkMessage>();
            CurrentStepModel = message.Data;

            NetworkServer.SendToAll(NetworkHandles.PresentStep, message);
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.UpdateHintTransform, networkMessage.ReadMessage<TransformNetworkMessage>());
        }
        private void OnNeedMoreInstructions(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.NeedMoreInstructions, networkMessage.ReadMessage<IDMessage>());
        }
        private void OnStepCompleted(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.StepCompleted, networkMessage.ReadMessage<IDMessage>());
        }
    }
}
