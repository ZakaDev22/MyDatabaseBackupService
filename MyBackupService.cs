﻿using System;
using System.ServiceProcess;

namespace MyDatabaseBackupService
{
    public partial class MyBackupService : ServiceBase
    {
        public MyBackupService()
        {
            InitializeComponent();

            // Create the Main Project Folders If Not Does Not Exist
            clsLog.CreateFolderIfDoesNotExist(clsGlobal.BackupFolder);
            clsLog.CreateFolderIfDoesNotExist(clsGlobal.logFolder);
        }

        protected override void OnStart(string[] args)
        {
            clsLog.LogServiceEvent("Service Started.");

            clsServiceDataAccess.BackupDatabase();
        }

        protected override void OnStop()
        {
            clsLog.LogServiceEvent("Service Sopped.");
        }

        public void StartInConsole()
        {
            OnStart(null); // Trigger OnStart logic
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine(); // Wait for user input to simulate service stopping
            OnStop(); // Trigger OnStop logic
            Console.ReadKey();
        }

    }
}
