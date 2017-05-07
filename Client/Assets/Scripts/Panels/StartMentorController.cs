using CollaborationEngine.Base;

namespace CollaborationEngine.Panels
{
    public class StartMentorController : Entity
    {
        public StartMentorView StartMentorViewPrefab;

        public void Start()
        {
            // Create start mentor view:
            var startMentorView = Instantiate(StartMentorViewPrefab);
            startMentorView.transform.SetParent(Application.View.MainCanvas.transform);

        }
    }
}
