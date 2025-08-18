using System;

namespace RcloneQBController.Services
{
    public interface IUserNotifierService
    {
        void ShowFriendlyError(Exception ex);
    }
}