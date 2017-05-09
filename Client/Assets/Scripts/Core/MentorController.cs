using CollaborationEngine.Base;
using CollaborationEngine.Panels;

namespace CollaborationEngine.Core
{
    public class MentorController : Controller
    {
        public StartMentorController StartMentorControllerPrefab;
        public EditorController EditorControllerPrefab;

        public void Start()
        {
            // TODO start server
            
            PresentStartScreen();
        }

        private void PresentStartScreen()
        {
            var controller = Instantiate(StartMentorControllerPrefab, transform);
            controller.OnTaskSelected += Controller_OnTaskSelected;
        }
        private void PresentEditorScreen()
        {
            var controller = Instantiate(EditorControllerPrefab, transform);
            controller.OnGoBack += Controller_OnGoBack;
        }

        private void Controller_OnTaskSelected(StartMentorController sender, Events.IDEventArgs e)
        {
            // Destroy start mentor controller:
            Destroy(sender.gameObject);

            PresentEditorScreen();
        }
        private void Controller_OnGoBack(EditorController sender, System.EventArgs eventArgs)
        {
            // Destroy editor controller:
            Destroy(sender.gameObject);

            PresentStartScreen();
        }
    }
}
