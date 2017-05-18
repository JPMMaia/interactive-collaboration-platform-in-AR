using CollaborationEngine.Base;
using CollaborationEngine.Network;
using CollaborationEngine.Panels;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.Core
{
    public class MentorController : Controller
    {
        #region Unity Editor
        public StartMentorController StartMentorControllerPrefab;
        public EditorController EditorControllerPrefab;
        public MentorNetworkManager MentorNetworkManager;
        #endregion

        #region Properties
        private TasksModel TasksModel
        {
            get { return Application.Model.Tasks; }
        }
        #endregion

        public void Start()
        {
            // Start a server and a client on the same application:
            MentorNetworkManager.StartHost();

            // Load all tasks:
            TasksModel.Load();

            // Present start screen:
            PresentStartScreen();
        }
        public void OnApplicationQuit()
        {
            TasksModel.Save();
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
            controller.NetworkManager = MentorNetworkManager;

            controller.OnGoBack += Controller_OnGoBack;
        }

        private void Controller_OnTaskSelected(StartMentorController sender, Events.IDEventArgs e)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            // Destroy start mentor controller:
            Destroy(sender.gameObject);

            PresentEditorScreen(e.ID);
        }
        private void Controller_OnGoBack(EditorController sender, System.EventArgs eventArgs)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            // Destroy editor controller:
            Destroy(sender.gameObject);

            PresentStartScreen();
        }
    }
}
