using CredentialManagement;

namespace RcloneQBController.Services
{
    public static class CredentialService
    {
        public static void SaveCredential(string target, string username, string password)
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

        public static Credential? RetrieveCredential(string target)
        {
            using (var cred = new Credential())
            {
                cred.Target = target;
                if (cred.Load())
                {
                    return cred;
                }
            }
            return null;
        }

        public static void DeleteCredential(string target)
        {
            using (var cred = new Credential())
            {
                cred.Target = target;
                cred.Delete();
            }
        }
    }
}