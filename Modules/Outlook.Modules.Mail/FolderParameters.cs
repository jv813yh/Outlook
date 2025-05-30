﻿using System.Net.NetworkInformation;
using System.Security.Policy;

namespace Outlook.Modules.Mail
{
    static class FolderParameters
    {
        public const string FolderKey = "Folder";
        public const string MailMessageKey = "id";

        public const string PersonalFolder = "Personal Folders";

        public const string Inbox = "Inbox";
        public const string Sent = "Sent";
        public const string Deleted = "Deleted";

        public const string MailListPath = "MailList";

        public const string MessageSent = "messageSent";

        public static string GetNavigationPath(string path, string key, string folder)
         => $"{path}?{key}={folder}";
    }
}
