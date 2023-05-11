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

namespace AppDI.Pags.AccesoRegistrado
{
    /// <summary>
    /// Lógica de interacción para VerFavs.xaml
    /// </summary>
    public partial class VerFavs : Page
    {
        private DB miDB;
        public VerFavs(DB db)
        {
            InitializeComponent();
            miDB = db;
        }

        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void SoporteTecnico_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SoporteTecnico());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblTituloFavs.Content = "Tienes un total de { "+miDB.comprobarTodosFavorito(miDB.NomUser)+" } de favoritos entre todas las categorias.";
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Se cargan los datos en los litsbox.
            lbPkmFav.ItemsSource = miDB.leerPkmsFavorito(miDB.NomUser);
            lbMovFav.ItemsSource = miDB.leerMovsFavorito(miDB.NomUser);
            lbTipFav.ItemsSource = miDB.leerTiposFavorito(miDB.NomUser);
        }
    }
}
