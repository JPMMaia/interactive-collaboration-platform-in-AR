using CollaborationEngine.Objects.Collision;
using CollaborationEngine.States;
using CollaborationEngine.States.Server;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    [RequireComponent(typeof(InputCollider))]
    public class TaskButton : MonoBehaviour
    {
        public void Awake()
        {
            Button = GetComponent<InputCollider>();
        }

        public void OnClick()
        {
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                var serverState = currentState as ServerCollaborationState;
                serverState.CurrentState = new StepState(serverState, Task);
            }
        }

        public InputCollider Button { get; private set; }
        public Task Task { get; set; }

        public Text Text;
    }
}
