using System;
using CollaborationEngine.Network;
using CollaborationEngine.Tasks;
using UnityEngine.Networking;

namespace CollaborationEngine.Feedback
{
    public class ApprenticeFeedbackModule
    {
        #region Classes
        public class StringMessage : MessageBase
        {
            public String Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(Data);
            }

            public override void Deserialize(NetworkReader reader)
            {
                Data = reader.ReadString();
            }
        }
        #endregion

        public void HelpWanted(String stepName)
        {
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.NeedMoreInstructions, new StringMessage{ Data = stepName });
        }

        public void StepCompleted(String stepName)
        {
            var networkClient = NetworkManager.singleton.client;
            networkClient.Send(NetworkHandles.StepCompleted, new StringMessage { Data = stepName });
        }
    }
}
