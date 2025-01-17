using System.ComponentModel;
using System.ServiceProcess;

namespace MyDatabaseBackupService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            // Service Process Installer
            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem // Use LocalSystem or a specific user account with permissions
            };

            // Service Installer
            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = "MyBackupService",
                DisplayName = "My Backup Service",
                Description = "A service that creates database backups on a schedule.",
                StartType = ServiceStartMode.Automatic // Automatically start the service on boot
            };

            // Set Dependencies
            serviceInstaller.ServicesDependedOn = new string[]
            {
            "MSSQLSERVER",    // SQL Server 
            "Schedule",       // Task Scheduler service
            "eventlog"        // Event Log service
            };

            // Add installers to collection
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
