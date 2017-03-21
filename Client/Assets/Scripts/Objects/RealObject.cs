namespace CollaborationEngine.Objects
{
    public class RealObject : SceneObject2
    {
        public RealObject() :
            base(ObjectLocator.Instance.StudyObjectPrefab)
        {
        }
    }
}
