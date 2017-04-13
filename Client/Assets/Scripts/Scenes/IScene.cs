using CollaborationEngine.Objects;

namespace CollaborationEngine.Scenes
{
    public interface IScene
    {
        void Add(SceneObject.Message sceneObject);
        void Remove(SceneObject.Message sceneObject);
        void Clear();
        void FixedUpdate();
        void FrameUpdate();
    }
}
