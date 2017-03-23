namespace CollaborationEngine.Objects
{
    public class RealObject : SceneObject2
    {
        public RealObject(SceneObject2.Data data) :
            base(ObjectLocator.Instance.StudyObjectPrefab, data)
        {
        }
    }
}
