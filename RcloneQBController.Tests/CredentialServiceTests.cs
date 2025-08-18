using RcloneQBController.Services;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class CredentialServiceTests
    {
        private const string TestTarget = "TestTarget";
        private const string TestPassword = "TestPassword";

        [TestCleanup]
        public void Cleanup()
        {
            var service = new CredentialService();
            service.DeleteCredential(TestTarget);
        }

        [TestMethod]
        public void SaveRetrieveDelete_CredentialLifecycle_IsValid()
        {
            // Save
            var service = new CredentialService();
            service.SaveCredential(TestTarget, "TestUser", TestPassword);

            // Retrieve
            var retrievedCredential = service.RetrieveCredential(TestTarget);
            Assert.IsNotNull(retrievedCredential);
            Assert.AreEqual(TestPassword, retrievedCredential.Password);

            // Delete
            service.DeleteCredential(TestTarget);
            var deletedCredential = service.RetrieveCredential(TestTarget);
            Assert.IsNull(deletedCredential);
        }
    }
}