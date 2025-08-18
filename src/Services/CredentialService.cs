using CredentialManagement;

namespace RcloneQBController.Services
{
    public class CredentialService : ICredentialService
    {
        public void SaveCredential(string target, string username, string password)
        {
            using (var cred = new Credential())
            {
                cred.Target = target;
                cred.Username = username;
                cred.Password = password;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }

        public Credential? RetrieveCredential(string target)
        {
            var cred = new Credential { Target = target };
            if (cred.Load())
            {
                return cred;
            }
            return null;
        }

        public void DeleteCredential(string target)
        {
            using (var cred = new Credential())
            {
                cred.Target = target;
                cred.Delete();
            }
        }
    }
}