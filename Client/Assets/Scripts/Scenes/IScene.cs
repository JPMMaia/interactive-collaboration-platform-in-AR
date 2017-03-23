using CollaborationEngine.Objects;

namespace CollaborationEngine.Scenes
{
    public interface IScene
    {
        void Add(SceneObject2.Data sceneObjectData);
        void Remove(SceneObject2.Data sceneObjectData);
        void Clear();
        void FixedUpdate();
        void FrameUpdate();
    }
}
