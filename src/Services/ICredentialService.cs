using CredentialManagement;

namespace RcloneQBController.Services
{
    public interface ICredentialService
    {
        void SaveCredential(string target, string username, string password);
        Credential? RetrieveCredential(string target);
        void DeleteCredential(string target);
    }
}