using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using CollaborationEngine.Hints;
using CollaborationEngine.Network;
using CollaborationEngine.Steps;
using UnityEngine.Networking;

namespace CollaborationEngine.Panels
{
    public class ARApprenticeController : Controller
    {
        public ARApprenticeView ARApprenticeViewPrefab;
        public Hint3DController Hint3DControllerPrefab;

        private ARApprenticeView _view;
        private readonly Dictionary<uint, Hint3DController> _hintControllers = new Dictionary<uint, Hint3DController>();

        public void Start()
        {
            // Instantiate view:
            _view = Instantiate(ARApprenticeViewPrefab, Application.View.MainCanvas.transform);

            // Subscribe to events:
            _view.OnNeedMoreInstructionsClicked += _view_OnNeedMoreInstructionsClicked;
            _view.OnCompletedTheStepClicked += _view_OnCompletedTheStepClicked;

            var networkClient = NetworkManager.singleton.client;
            networkClient.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
            networkClient.RegisterHandler(NetworkHandles.UpdateHintTransform, OnHintTransformUpdate);
        }
        public void OnDestroy()
        {
            if(_view)
                Destroy(_view.gameObject);
        }

        private void CreateHintController(HintModel hintModel)
        {
            var hintController = Instantiate(Hint3DControllerPrefab, transform);

            hintController.HintModel = hintModel;

            _hintControllers.Add(hintModel.ID, hintController);
        }

        private void _view_OnNeedMoreInstructionsClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void _view_OnCompletedTheStepClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            var stepModel = networkMessage.ReadMessage<StepModelNetworkMessage>().Data;

            // Reparent:
            stepModel.gameObject.transform.SetParent(Application.Model.Tasks.transform);
            foreach (var hintModel in stepModel.Hints)
                hintModel.Value.transform.SetParent(stepModel.transform);

            // Destroy previous step hint controllers:
            foreach (var hintController in _hintControllers)
                Destroy(hintController.Value.gameObject);
            _hintControllers.Clear();

            // Create hint controllers:
            foreach (var hint in stepModel.Hints)
                CreateHintController(hint.Value);
        }
        private void OnHintTransformUpdate(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<TransformNetworkMessage>();

            var hintView = _hintControllers[data.ID].Hint3DView;
            hintView.transform.localPosition = data.Position;
            hintView.transform.localRotation = data.Rotation;
            hintView.transform.localScale = data.Scale;
        }
    }
}
