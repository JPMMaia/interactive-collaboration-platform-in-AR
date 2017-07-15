using CollaborationEngine.Hints;
using NUnit.Framework;
using UnityEngine;

namespace CollaborationEngine.Tasks.Editor
{
    public class HintTests
    {
        [Test]
        public void HintTestsDeepCopy()
        {
            {
                var expected = CreateImageHint(1, 2, 3, "Test1", Vector3.one, Quaternion.identity, Vector3.one, ImageHintType.Axe);
                var actual = expected.DeepCopy(expected.transform.parent, expected.TaskID, expected.StepID);

                AssertHintsAreEqual(expected, actual, expected.TaskID, expected.StepID);
            }

            {
                var expected = CreateTextHint(1, 2, 3, "Test2", Vector3.one, Quaternion.identity, Vector3.one);
                var actual = expected.DeepCopy(expected.transform.parent, expected.TaskID, expected.StepID);

                AssertHintsAreEqual(expected, actual, expected.TaskID, expected.StepID);
            }
        }

        public static void AssertHintsAreEqual(HintModel expected, HintModel actual, uint expectedTaskID, uint expectedStepID)
        {
            Assert.AreEqual(expected.Type, actual.Type);

            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expectedTaskID, actual.TaskID);
            Assert.AreEqual(expectedStepID, actual.StepID);
            UnityEngine.Assertions.Assert.AreEqual(expected.LocalPosition, actual.LocalPosition);
            UnityEngine.Assertions.Assert.AreEqual(expected.LocalRotation, actual.LocalRotation);
            UnityEngine.Assertions.Assert.AreEqual(expected.LocalScale, actual.LocalScale);

            if (expected.Type == HintType.Image)
            {
                Assert.AreEqual(((ImageHintModel) expected).ImageHintType, ((ImageHintModel) actual).ImageHintType);
            }
        }

        private static void FillHintProperties(HintModel hint, uint id, uint taskID, uint stepID, string name, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            hint.ID = id;
            hint.TaskID = taskID;
            hint.StepID = stepID;
            hint.Name = name;
            hint.LocalPosition = position;
            hint.LocalRotation = rotation;
            hint.LocalScale = scale;
        }
        public static TextHintModel CreateTextHint(uint id, uint taskID, uint stepID, string name, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var gameObject = new GameObject();

            var textHint = gameObject.AddComponent<TextHintModel>();
            FillHintProperties(textHint, id, taskID, stepID, name, position, rotation, scale);
            
            return textHint;
        }
        public static ImageHintModel CreateImageHint(uint id, uint taskID, uint stepID, string name, Vector3 position, Quaternion rotation, Vector3 scale, ImageHintType imageHintType)
        {
            var gameObject = new GameObject();

            var imageHint = gameObject.AddComponent<ImageHintModel>();
            FillHintProperties(imageHint, id, taskID, stepID, name, position, rotation, scale);
            imageHint.ImageHintType = imageHintType;

            return imageHint;
        }
    }
}
