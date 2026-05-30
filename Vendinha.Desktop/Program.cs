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
            Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection",
                "Server=localhost;Port=5432;User Id=postgres;Password=bmols123;Database=vendinha");

            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}