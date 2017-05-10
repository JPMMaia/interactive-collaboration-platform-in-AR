using System;
using NUnit.Framework;
using UnityEngine;

namespace CollaborationEngine.Tasks.Editor
{
    public class TaskTests
    {
        [Test]
        public void TaskTestsSimplePasses()
        {
            // CreateStep task:
            var savedTask = CreateTask(1, "TestTask");

            // Save task:
            var path = String.Format("Saved/Tasks/{0}/", savedTask.ID);
            savedTask.Save(path);

            // Load a task:
            var loadedTask = CreateTask(0, String.Empty);
            loadedTask.Load(path);

            Assert.AreEqual(savedTask.ID, loadedTask.ID);
            Assert.AreEqual(savedTask.Name, loadedTask.Name);
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
