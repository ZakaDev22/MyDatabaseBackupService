using System.ServiceProcess;

namespace MyDatabaseBackupService
{
    public partial class MyBackupService : ServiceBase
    {
        public MyBackupService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        public void StartInConsole()
        {

        }

    }
}
