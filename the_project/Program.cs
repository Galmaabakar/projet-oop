using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace the_project
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

            // Cree la base SQLite au premier lancement, puis charge les donnees.
            try
            {
                Veritabani.Hazirla();
                Veriler.Yukle();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Veritabani acilamadi:" + Environment.NewLine + hata.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Form1());
        }
    }
}
