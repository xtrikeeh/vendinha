using System;
using System.Windows.Forms;
using Vendinha.Desktop.Screens;

namespace Vendinha.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}