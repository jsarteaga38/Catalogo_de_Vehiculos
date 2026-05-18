using Catalogo_de_Vehiculo.Presentation.Forms;
using WinForms = System.Windows.Forms;

namespace Catalogo_de_Vehiculo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            WinForms.Application.EnableVisualStyles();
            WinForms.Application.SetCompatibleTextRenderingDefault(false);
            WinForms.Application.Run(new FormPrincipal());
        }
    }
}