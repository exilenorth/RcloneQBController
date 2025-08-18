using Moq;
using RcloneQBController.Models;
using RcloneQBController.Services;
using System.Threading.Tasks;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class ScriptExecutionWorkflowTests
    {
        private TestSetup _testSetup;

        [TestInitialize]
        public void Setup()
        {
            _testSetup = new TestSetup();
        }

        [TestMethod]
        public async Task RunNow_ExecutesJob_And_Notifies()
        {
            // Arrange
            var job = new RcloneJobConfig { Name = "TestJob" };
            _testSetup.MainViewModel.Jobs.Add(job);

            // Act
            var tcs = new TaskCompletionSource<object>();
            _testSetup.MockScriptRunnerService.Setup(s => s.RunRcloneJobAsync(job, It.IsAny<Action<string>>()))
                .Returns(Task.CompletedTask)
                .Callback(() => tcs.SetResult(null));

            _testSetup.MainViewModel.RunJobCommand.Execute(job);
            await tcs.Task;


            // Assert
            _testSetup.MockScriptRunnerService.Verify(s => s.RunRcloneJobAsync(job, It.IsAny<System.Action<string>>()), Times.Once);
            _testSetup.MockNotificationService.Verify(s => s.ShowNotification("Success", $"Job {job.Name} completed successfully."), Times.Once);
        }
    }
}