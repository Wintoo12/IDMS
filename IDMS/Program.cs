using IDMS.Admin;
using IDMS.Admin.Manage_Customer;
using IDMS.Staff.Manage_Customer;
using IDMS.Staff.Process_Order.Installations;
using IDMS.Staff.Process_Order.Supplies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS
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
            Application.Run(new Login());
        }
    }
}
