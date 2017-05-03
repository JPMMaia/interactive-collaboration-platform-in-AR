using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public interface ISerializable
    {
        void Serialize(NetworkWriter writer);
        void Deserialize(NetworkReader reader);
    }
}
