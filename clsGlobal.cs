﻿using System.Configuration;
using System.IO;

namespace MyDatabaseBackupService
{
    internal class clsGlobal
    {
        public static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public static string BackupFolder = ConfigurationManager.AppSettings["BackupFolder"];
        public static string logFolder = ConfigurationManager.AppSettings["LogFolder"];
        public static string logFilePath = Path.Combine(logFolder, "ServiceLog.txt");

    }
}