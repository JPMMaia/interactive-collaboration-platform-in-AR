using CollaborationEngine.Steps;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
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
        private uint _currentStepOrder;
        private int _currentImageTargetIndex;

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
            var message = new StepModelNetworkMessage(_currentImageTargetIndex, _currentStepOrder, CurrentStepModel);
            NetworkServer.SendToAll(NetworkHandles.Initialize, message);
        }
        private void OnPresentStep(NetworkMessage networkMessage)
        {
            var message = networkMessage.ReadMessage<StepModelNetworkMessage>();
            _currentImageTargetIndex = message.ImageTargetIndex;
            CurrentStepModel = message.Data;
            _currentStepOrder = message.StepOrder;

            NetworkServer.SendToAll(NetworkHandles.PresentStep, message);
        }
        private void OnUpdateInstruction(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.UpdateHintTransform, networkMessage.ReadMessage<TransformNetworkMessage>());
        }
        private void OnNeedMoreInstructions(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.NeedMoreInstructions, networkMessage.ReadMessage<IDNetworkMessage>());
        }
        private void OnStepCompleted(NetworkMessage networkMessage)
        {
            NetworkServer.SendToAll(NetworkHandles.StepCompleted, networkMessage.ReadMessage<IDNetworkMessage>());
        }
    }
}
