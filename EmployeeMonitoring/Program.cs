using EmployeeMonitoring.Models.Forms;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace EmployeeMonitoring
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            DatabaseInitializer.InitializeDatabase(connectionString);

            Application.Run(new EmployeeListForm());
        }
    }

}
