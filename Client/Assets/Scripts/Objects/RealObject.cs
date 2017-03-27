namespace CollaborationEngine.Objects
{
    public class RealObject : SceneObject
    {
        public RealObject(SceneObject.Data data) :
            base(ObjectLocator.Instance.StudyObjectPrefab, data)
        {
        }
    }
}
