using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AppDI.Pags
{
    /// <summary>
    /// Lógica de interacción para Primera.xaml
    /// </summary>
    public partial class Primera : Page
    {
        /// <summary>
        /// Constructor, no se le pasa nada por parámetro.
        /// </summary>
        public Primera()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Acción para navegar. Le meto lo que quiero que se ejecute en el powerShell y el argumento, además del link que tiene mi hyperLink en wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process proceso = new Process();
            proceso.StartInfo.UseShellExecute = true;
            proceso.StartInfo.Arguments = "brave.exe";
            proceso.StartInfo.FileName = e.Uri.AbsoluteUri;
            
            proceso.Start();
            e.Handled = true;
        }

        /// <summary>
        /// Al hacer clicl al elemento "Iniciar sesión" que llevará a la página de iniciar sesión.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileMenuItem3_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InicioSesion());
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Por favor, seleccione algún botón de la barra superior para ir a diferentes apartados de la aplicación");
        }

        /// <summary>
        /// Al hacer click al elemento "Acceso Gratuito" navegará a la ventana de acceso gratuito.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccGratuito_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Gratuito());
        }

        /// <summary>
        /// Al hacer click al elemento "Registro" navegará a la ventana de registro dónde podrás crear una cuenta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registro_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Registro());
        }
    }
}
