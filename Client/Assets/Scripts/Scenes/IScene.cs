using CollaborationEngine.Objects;

namespace CollaborationEngine.Scenes
{
    public interface IScene
    {
        void Add(SceneObject2 sceneObject);
        void Remove(SceneObject2 sceneObject);
        void Clear();
        void FixedUpdate();
        void FrameUpdate();
    }
}
