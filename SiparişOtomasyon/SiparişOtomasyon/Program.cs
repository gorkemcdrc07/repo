using System;
using System.Windows.Forms;

namespace SiparişOtomasyon
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AnaSayfa());  // AnaSayfa formunu aç
        }
    }
}
