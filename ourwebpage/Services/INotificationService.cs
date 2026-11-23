using System.Collections.Generic;
using ourwebpage.Models;

namespace ourwebpage.Services
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetByRoleId(int roleId);
        void CreateNotification(int roleId, string title, string message);
        void MarkAsRead(int notificationId);
    }
}