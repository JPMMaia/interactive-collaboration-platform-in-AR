using System;
using CollaborationEngine.Steps;
using NUnit.Framework;
using UnityEngine;

namespace CollaborationEngine.Tasks.Editor
{
    public class TaskTests
    {
        public static StepModel StepModelPrefab
        {
            get
            {
                var gameObject = new GameObject();
                return gameObject.AddComponent<StepModel>();
            }
        }

        [Test]
        public void TaskTestsLoadSave()
        {
            // Create task:
            var savedTask = CreateTask(1, "TestTask");

            // Save task:
            var path = String.Format("Saved/Tasks/{0}/", savedTask.ID);
            savedTask.Save(path);

            // Load a task:
            var loadedTask = CreateTask(0, String.Empty);
            loadedTask.Load(path);

            AssertTasksAreEqual(savedTask, loadedTask);
        }

        [Test]
        public void TaskTestsDeepCopy()
        {
            {
                var expected = CreateTask(1, "Test1");
                expected.StepModelPrefab = StepModelPrefab;

                {
                    var step = expected.CreateStep();
                    step.Name = "Test2";

                    {
                        var hint = step.CreateHint(StepTests.ImageHintModelPrefab);
                        step.DuplicateHint(hint.ID);
                        step.DuplicateHint(hint.ID);
                        step.DuplicateHint(hint.ID);
                    }
                }

                var actual = expected.DeepCopy(expected.transform.parent);

                AssertTasksAreEqual(expected, actual);
            }
        }

        public static void AssertTasksAreEqual(TaskModel expected, TaskModel actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);

            using (var expectedStepIt = expected.Steps.GetEnumerator())
            {
                using (var actualStepIt = actual.Steps.GetEnumerator())
                {
                    while (true)
                    {
                        var expectedNext = expectedStepIt.MoveNext();
                        var actualNext = actualStepIt.MoveNext();

                        Assert.IsTrue(expectedNext == actualNext);
                        if (!expectedNext)
                            break;

                        StepTests.AssertStepsAreEqual(expectedStepIt.Current.Value, actualStepIt.Current.Value, actual.ID);
                    }
                }
            }
        }

        private TaskModel CreateTask(uint id, string name)
        {
            var gameObject = new GameObject();

            var taskModel = gameObject.AddComponent<TaskModel>();
            taskModel.ID = id;
            taskModel.Name = name;

            return taskModel;
        }
    }
}
