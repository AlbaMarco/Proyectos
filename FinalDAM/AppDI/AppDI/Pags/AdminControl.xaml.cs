using AppDI.Pags.PanelAdmin;
using AppDI.Pags.PanelAdmin.Admin;
using AppDI.Pags.PanelAdmin.SuperAdmin;
using AppDI.Recursos;
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
    /// Lógica de interacción para AdminControl.xaml
    /// </summary>
    public partial class AdminControl : Page
    {
        /// <summary>
        /// Variable de tipo base de datos, utilizada para el contrl de la base de datos.
        /// </summary>
        private DB miDB;
        /// <summary>
        /// Constructor que se la pasa por parámetros un objeto de tipo base de datos.
        /// </summary>
        /// <param name="dB"></param>
        public AdminControl(DB dB)
        {
            InitializeComponent();
            miDB = dB;
            ComprobarAdmin(dB.EsSuperAdmin());
        }
        /// <summary>
        /// Evento que comprueba el nivel del adimnistrados, según que nivel sea se habilitarán una u otras opciones. 
        /// </summary>
        /// <param name="nivel"></param>
        private void ComprobarAdmin(bool nivel)
        {
            if(!nivel) // Si es super Administrador.
            {
                expand.btnEliUser.Visibility = Visibility.Hidden;
                expand.btnModAdmin.Visibility = Visibility.Hidden;
                expand.btnLogsAdmin.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Al carga esta página, pondrá un mensaje de bienvenida al usuario conectado al panel de control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tblockBien.Text = "Bienvenid@ " + miDB.NomUser;
            this.NavigationService.RemoveBackEntry();
        }

        /// <summary>
        /// Acción de doble click en mi expander.
        /// Dependiendo de donde se haga el doble click, se irá a una parte u otra gracias al Click del propio User Control "Expander" que modifica la propiedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expand_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(expand.NumBtn == 1)
            {
                frameExpander.Navigate(new ListaUsuarios(miDB));
            } else if (expand.NumBtn == 2)
            {
                frameExpander.Navigate(new ModificacionNivel(miDB));
            }
            else if (expand.NumBtn == 3)
            {
                frameExpander.Navigate(new AddNuevosUsers(miDB));
            }
            else if (expand.NumBtn == 4)
            {
                frameExpander.Navigate(new CrearEquiposAdmin(miDB));
            }
            else if (expand.NumBtn == 5)
            {
                frameExpander.Navigate(new BanearPkm(miDB));
            }
            else if (expand.NumBtn == 6)
            {
                frameExpander.Navigate(new VerSoporteTecnico(miDB));  
            } 
            else if (expand.NumBtn == 7)
            {
                frameExpander.Navigate(new EliminarUsuarios(miDB));
            } 
            else if (expand.NumBtn == 8)
            {
                frameExpander.Navigate(new ModificacionAdmin(miDB));
            } 
            else if (expand.NumBtn == 9)
            {
                frameExpander.Navigate(new VerLogs(miDB));
            }
        }

        /// <summary>
        /// Evento que ayuda a cuando se hace click sobre un elemento, en este caso en el apartado de Inicio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Evento que abre un proceso, en este caso de tipo shell, para abrir el navegador de microsoft edge para llevar a la página que se le haya indicado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process proceso = new Process();
            proceso.StartInfo.UseShellExecute = true;
            proceso.StartInfo.Arguments = "msedge";
            proceso.StartInfo.FileName = e.Uri.AbsoluteUri;

            proceso.Start();
            e.Handled = true;
        }
    }
}
