using RcloneQBController.Services;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class UserNotifierServiceTests
    {
        [TestMethod]
        public void GetFriendlyMessage_WithRcloneExeNotFound_ReturnsCorrectMessage()
        {
            var exception = new FileNotFoundException("Could not find rclone.exe", "rclone.exe");
            var message = (string)typeof(UserNotifierService).GetMethod("GetFriendlyMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { exception });
            Assert.IsTrue(message.Contains("rclone.exe not found"));
        }

        [TestMethod]
        public void GetFriendlyMessage_WithGenericException_ReturnsDefaultMessage()
        {
            var exception = new Exception("Something went wrong");
            var message = (string)typeof(UserNotifierService).GetMethod("GetFriendlyMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { exception });
            Assert.IsTrue(message.Contains("An unexpected error occurred"));
        }
    }
}