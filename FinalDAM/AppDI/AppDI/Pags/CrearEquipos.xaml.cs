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
    /// Lógica de interacción para CrearEquipos.xaml
    /// </summary>
    public partial class CrearEquipos : Page
    {
        private DB miDB;
        public CrearEquipos(DB db)
        {
            InitializeComponent();
            miDB = db;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(miDB.SaberEquipos(miDB.NomUser) != "-1")
            {
                lblEquipos.Content = "Actualmente tienes [ " + miDB.SaberEquipos(miDB.NomUser) + " ] equipos creados";
                btnRecargaEquipos.Visibility = Visibility.Hidden;
            } else
            {
                lblEquipos.Content = "Hubo un error al leer la base da datos. Por favor, recargue la página";
                btnRecargaEquipos.Visibility = Visibility.Visible;
                btnCrearEquipos.Visibility = Visibility.Hidden;
            }
            
        }

        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Primera());
        }

        private void SoporteTecnico_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SoporteTecnico());
        }

        private void AccReg_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnRecargaEquipos_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Refresh();
        }
    }
}
