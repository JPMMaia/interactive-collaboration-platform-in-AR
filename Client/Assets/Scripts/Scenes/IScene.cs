using CollaborationEngine.Objects;

namespace CollaborationEngine.Scenes
{
    public interface IScene
    {
        void Add(SceneObject.Data sceneObjectData);
        void Remove(SceneObject.Data sceneObjectData);
        void Clear();
        void FixedUpdate();
        void FrameUpdate();
    }
}
