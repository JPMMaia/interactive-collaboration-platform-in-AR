using CollaborationEngine.Hints;
using CollaborationEngine.Steps;
using NUnit.Framework;
using UnityEngine;

namespace CollaborationEngine.Tasks.Editor
{
    public class StepTests
    {
        public static ImageHintModel ImageHintModelPrefab
        {
            get
            {
                var gameObject = new GameObject();
                return gameObject.AddComponent<ImageHintModel>();
            }
        }

        [Test]
        public void StepTestsDeepCopy()
        {
            {
                var expected = CreateStep(1, 2, "Test1");

                {
                    var hint = expected.CreateHint(ImageHintModelPrefab);
                    expected.DuplicateHint(hint.ID);
                    expected.DuplicateHint(hint.ID);
                    expected.DuplicateHint(hint.ID);
                }

                var actual = expected.DeepCopy(expected.transform.parent, expected.TaskID);

                AssertStepsAreEqual(expected, actual, expected.TaskID);
            }
        }

        public static void AssertStepsAreEqual(StepModel expected, StepModel actual, uint expectedTaskID)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expectedTaskID, actual.TaskID);

            using (var expectedHintIt = expected.Hints.GetEnumerator())
            {
                using (var actualHintIt = actual.Hints.GetEnumerator())
                {
                    while (true)
                    {
                        var expectedNext = expectedHintIt.MoveNext();
                        var actualNext = actualHintIt.MoveNext();

                        Assert.IsTrue(expectedNext == actualNext);
                        if (!expectedNext)
                            break;

                        HintTests.AssertHintsAreEqual(expectedHintIt.Current.Value, actualHintIt.Current.Value, expectedTaskID, actual.ID);
                    }
                }
            }
        }

        private static StepModel CreateStep(uint id, uint taskID, string name)
        {
            var gameObject = new GameObject();

            var step = gameObject.AddComponent<StepModel>();
            step.ID = id;
            step.TaskID = taskID;
            step.Name = name;

            return step;
        }
    }
}
