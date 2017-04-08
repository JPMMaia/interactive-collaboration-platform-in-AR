using System.IO;
using JetBrains.Annotations;

namespace CollaborationEngine.Network
{
    public interface ISerializable
    {
        void ReadFromMemoryStream(MemoryStream memoryStream);
        MemoryStream WriteToMemoryStream();
    }
}
