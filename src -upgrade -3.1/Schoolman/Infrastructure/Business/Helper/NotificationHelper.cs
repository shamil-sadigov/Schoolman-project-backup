using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Helper
{
    public static class NotificationHelper
    {
        public static StringBuilder AddConfirmationUrl(this StringBuilder htmlTemplate, string url) =>
             htmlTemplate.Replace("aspnet-confirmation-url", url);


        public static StringBuilder AddUserName(this StringBuilder htmlTemplate, string token) =>
             htmlTemplate.Replace("aspnet-username", token);
    }
}
