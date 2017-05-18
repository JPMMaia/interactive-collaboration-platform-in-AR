using System;
using CollaborationEngine.Hints;
using NUnit.Framework;
using UnityEngine;

namespace CollaborationEngine.Tasks.Editor
{
    public class HintTests
    {
        [Test]
        public void TaskTestsSimplePasses()
        {
            {
                var expected = CreateImageHint(1, 2, 3, "Test1", Vector3.one, Quaternion.identity, Vector3.one, ImageHintType.Axe);
                var actual = expected.DeepCopy(expected.transform.parent, expected.TaskID, expected.StepID);

                AssertHintsAreEqual(expected, actual);
            }

            {
                var expected = CreateTextHint(1, 2, 3, "Test1", Vector3.one, Quaternion.identity, Vector3.one);
                var actual = expected.DeepCopy(expected.transform.parent, expected.TaskID, expected.StepID);

                AssertHintsAreEqual(expected, actual);
            }
        }

        public static void AssertHintsAreEqual(HintModel expected, HintModel actual)
        {
            Assert.AreEqual(expected.Type, actual.Type);

            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.TaskID, actual.TaskID);
            Assert.AreEqual(expected.StepID, actual.StepID);
            UnityEngine.Assertions.Assert.AreEqual(expected.Position, actual.Position);
            UnityEngine.Assertions.Assert.AreEqual(expected.Rotation, actual.Rotation);
            UnityEngine.Assertions.Assert.AreEqual(expected.Scale, actual.Scale);

            if (expected.Type == HintType.Image)
            {
                Assert.AreEqual(((ImageHintModel) expected).ImageHintType, ((ImageHintModel) actual).ImageHintType);
            }
        }

        private void FillHintProperties(HintModel hint, uint id, uint taskID, uint stepID, string name, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            hint.ID = id;
            hint.TaskID = taskID;
            hint.StepID = stepID;
            hint.Name = name;
            hint.Position = position;
            hint.Rotation = rotation;
            hint.Scale = scale;
        }
        private TextHintModel CreateTextHint(uint id, uint taskID, uint stepID, string name, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var gameObject = new GameObject();

            var textHint = gameObject.AddComponent<TextHintModel>();
            FillHintProperties(textHint, id, taskID, stepID, name, position, rotation, scale);
            
            return textHint;
        }
        private ImageHintModel CreateImageHint(uint id, uint taskID, uint stepID, string name, Vector3 position, Quaternion rotation, Vector3 scale, ImageHintType imageHintType)
        {
            var gameObject = new GameObject();

            var imageHint = gameObject.AddComponent<ImageHintModel>();
            FillHintProperties(imageHint, id, taskID, stepID, name, position, rotation, scale);
            imageHint.ImageHintType = imageHintType;

            return imageHint;
        }
    }
}
