using System;
using System.ServiceProcess;

namespace MyDatabaseBackupService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Running in console mode
                Console.WriteLine("Running in console mode...");
                MyBackupService service = new MyBackupService();
                service.StartInConsole();
            }
            else
            {
                // Running as a Windows Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MyBackupService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
