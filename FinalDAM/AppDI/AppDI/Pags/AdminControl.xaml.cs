using AppDI.Pags.PanelAdmin;
using AppDI.Recursos;
using System;
using System.Collections.Generic;
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

        private void ComprobarAdmin(bool nivel)
        {
            if(nivel) // Si es super Administrador.
            {
                expand.btnAddUser.Visibility = Visibility.Visible;
                expand.btnEliUser.Visibility = Visibility.Visible;
                expand.btnListaUsers.Visibility = Visibility.Visible;
                expand.btnModAdmin.Visibility = Visibility.Visible;
                expand.btnModNivel.Visibility = Visibility.Visible;
            } else
            {
                expand.btnAddUser.Visibility = Visibility.Visible;
                expand.btnEliUser.Visibility = Visibility.Hidden; // Eliminar usuarios. 2
                expand.btnListaUsers.Visibility = Visibility.Visible;
                expand.btnModAdmin.Visibility = Visibility.Hidden; // Modificar administrador. 2
                expand.btnModNivel.Visibility = Visibility.Visible;
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
                frameExpander.Navigate(new EliminarUsuarios(miDB));
            }
            else if (expand.NumBtn == 5)
            {
                frameExpander.Navigate(new ModificacionAdmin(miDB));
            }
        }

        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
