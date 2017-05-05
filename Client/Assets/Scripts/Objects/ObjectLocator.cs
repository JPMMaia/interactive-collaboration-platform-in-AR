using CollaborationEngine.Camera;
using CollaborationEngine.UI.Console;
using CollaborationEngine.UI.Feedback;
using CollaborationEngine.UI.Generic;
using CollaborationEngine.UI.Hint;
using CollaborationEngine.UI.Steps;
using CollaborationEngine.UI.Tasks;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class ObjectLocator : MonoBehaviour
    {
        public UnityEngine.Camera MainCamera
        {
            get { return UnityEngine.Camera.main; }
        }

        public Shader StandardShader;

        public GameObject ServerRoot;
        public GameObject ClientRoot;
        public GameObject SceneRoot;

        public GameObject StudyObjectPrefab;
        public GameObject IndicationPrefab;
        public GameObject IndicationToolsPrefab;
        public TasksPanel ServerTaskPanelPrefab;
        public TasksPanel ClientTaskPanelPrefab;
        public EditTaskPanel EditTaskPanelPrefab;
        public EditStepPanel EditStepPanelPrefab;
        public StepsPanel ServerStepsPanelPrefab;
        public StepsPanel ClientStepsPanelPrefab;
        public GameObject TextInstructionPrefab;
        public FeedbackPanel FeedbackPanelPrefab;

        public RectTransform UICanvas;
        public HintText HintText;
        public FixedPanel LeftPanel;
        public FixedPanel RightPanel;
        public CameraManager CameraManager;
        public ConsoleController ConsoleController;

        private static ObjectLocator _instance;
        public static ObjectLocator Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType(typeof(ObjectLocator)) as ObjectLocator;

                return _instance;
            }
        }

        private ObjectLocator()
        {
        }
    }
}