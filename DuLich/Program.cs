using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using admin;

namespace DuLich
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

            Application.Run(new DangNhap());

            //if (string.IsNullOrEmpty(Properties.Settings.Default.username))
            //{
            //    Application.Run(new Signup());

            //}
            //else if (!string.IsNullOrEmpty(Properties.Settings.Default.password))
            //{
            //    Application.Run(new TrangChu());
            //}
            //else
            //{
            //    Application.Run(new Login());
            //}
        }
    }
}
