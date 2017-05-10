using CollaborationEngine.Base;
using CollaborationEngine.Panels;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.Core
{
    public class MentorController : Controller
    {
        #region Unity Editor
        public StartMentorController StartMentorControllerPrefab;
        public EditorController EditorControllerPrefab;
        #endregion

        #region Properties
        private TasksModel TasksModel
        {
            get { return Application.Model.Tasks; }
        }
        #endregion

        public void Start()
        {
            // TODO start server

            // Load all tasks:
            TasksModel.Load();

            // Present start screen:
            PresentStartScreen();
        }

        private void PresentStartScreen()
        {
            var controller = Instantiate(StartMentorControllerPrefab, transform);
            controller.OnTaskSelected += Controller_OnTaskSelected;
        }
        private void PresentEditorScreen(uint taskID)
        {
            var controller = Instantiate(EditorControllerPrefab, Application.View.MainCanvas.transform);
            controller.TaskID = taskID;

            controller.OnGoBack += Controller_OnGoBack;
        }

        private void Controller_OnTaskSelected(StartMentorController sender, Events.IDEventArgs e)
        {
            // Destroy start mentor controller:
            Destroy(sender.gameObject);

            PresentEditorScreen(e.ID);
        }
        private void Controller_OnGoBack(EditorController sender, System.EventArgs eventArgs)
        {
            // Destroy editor controller:
            Destroy(sender.gameObject);

            PresentStartScreen();
        }
    }
}
