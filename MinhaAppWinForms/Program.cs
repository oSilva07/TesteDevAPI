using System;
using System.Windows.Forms;

namespace MinhaAppWinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Nota: Se o .NET 8 reclamar do ApplicationConfiguration, 
            // podemos usar o padrão tradicional que funciona em qualquer versão:
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}