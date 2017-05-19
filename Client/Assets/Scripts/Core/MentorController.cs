using System.IO;
using CollaborationEngine.Base;
using CollaborationEngine.Cameras;
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
        public CameraManager CameraManager;
        #endregion

        #region Properties
        private TasksModel TasksModel
        {
            get { return Application.Model.Tasks; }
        }
        private uint CurrentTaskID
        {
            set
            {
                // If a task was selected, save camera configurations:
                if (_currentTaskID != NullID)
                    SaveCameraConfigurations(_currentTaskID);

                _currentTaskID = value;

                // If a task is selected, load camera configurations:
                if (_currentTaskID != NullID)
                    LoadCameraConfigurations(_currentTaskID);
            }
        }
        #endregion

        private const uint NullID = uint.MaxValue;
        private uint _currentTaskID = NullID;

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

            // Unset task ID, in order to proper save the configurations:
            CurrentTaskID = NullID;
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

            // Set task ID:
            CurrentTaskID = e.ID;
            
        }
        private void Controller_OnGoBack(EditorController sender, System.EventArgs eventArgs)
        {
            // Unset task ID:
            CurrentTaskID = NullID;

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            // Destroy editor controller:
            Destroy(sender.gameObject);

            PresentStartScreen();
        }

        private void SaveCameraConfigurations(uint taskID)
        {
            var directory = TasksModel.GenerateSavedTaskPath(taskID);

            // Create directory if it doesn't exist:
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Serialize cameras:
            var data = new MemoryStream();
            CameraManager.Serialize(new BinaryWriter(data));

            // Write to file:
            var file = directory + "Cameras.data";
            using (var stream = File.OpenWrite(file))
            {
                using (var binaryStream = new BinaryWriter(stream))
                {
                    binaryStream.Write(data.ToArray(), 0, (int)data.Length);
                }
            }
        }
        private void LoadCameraConfigurations(uint taskID)
        {
            var directory = TasksModel.GenerateSavedTaskPath(taskID);

            // Create directory if it doesn't exist:
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Read file:
            var file = directory + "Cameras.data";
            if (File.Exists(file))
            {
                MemoryStream data;

                using (var stream = File.OpenRead(file))
                {
                    using (var binaryStream = new BinaryReader(stream))
                    {
                        var bytes = binaryStream.ReadBytes((int)stream.Length);
                        data = new MemoryStream(bytes);
                    }
                }

                // Deserialize object:
                CameraManager.Deserialize(new BinaryReader(data));
            }
        }
    }
}
