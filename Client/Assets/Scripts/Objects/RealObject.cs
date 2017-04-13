namespace CollaborationEngine.Objects
{
    public class RealObject : SceneObject
    {
        public RealObject() :
            base(ObjectLocator.Instance.StudyObjectPrefab, SceneObjectType.Real)
        {
        }
    }
}
