using System;
using System.IO;
using UnityEngine;

namespace CollaborationEngine.Experiments
{
    public class LegoExperimentPrecision : MonoBehaviour
    {
        [Serializable]
        public struct PieceTransform
        {
            public Vector3 Position;
            public Vector3 Rotation;
            public Vector3 Scale;
        }
        public PieceTransform[] FinalPiecesTransforms;

        public void OnApplicationQuit()
        {
            // Create directory if it doesn't exist:
            const string directory = "Experiments/";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Write to file:
            var file = directory + String.Format("{0:yyyy-mm-dd_hh-MM-ss}.data", DateTime.Now);
            using (var stream = File.OpenWrite(file))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    for (var i = transform.childCount - FinalPiecesTransforms.Length; i < transform.childCount; ++i)
                    {
                        var actualTransform = transform.GetChild(i);
                        streamWriter.WriteLine("{0}\t{1}\t{2}", actualTransform.localPosition.x, actualTransform.localPosition.y, actualTransform.localPosition.z);
                        streamWriter.WriteLine("{0}\t{1}\t{2}", actualTransform.eulerAngles.x, actualTransform.eulerAngles.y, actualTransform.eulerAngles.z);
                        streamWriter.WriteLine("{0}\t{1}\t{2}", actualTransform.localScale.x, actualTransform.localScale.y, actualTransform.localScale.z);
                    }
                }
            }
        }
    }
}
