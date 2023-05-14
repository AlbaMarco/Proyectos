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
        /// <summary>
        /// Variable de tipo base de datos.
        /// </summary>
        private DB miDB;
        /// <summary>
        /// Constructor de la ventana de ver favotiros.
        /// </summary>
        /// <param name="db"></param>
        public VerFavs(DB db)
        {
            InitializeComponent();
            miDB = db;
        }

        /// <summary>
        /// Evento de click que llevará a la página anterior a la que se ha estado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Evento ded click en la barra de nevagación que llevará a la página de soprote técnico.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoporteTecnico_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SoporteTecnico());
        }

        /// <summary>
        /// Evento de cuando la página se recarga, donde se mostrará todos los favoritos que tiene un usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblTituloFavs.Content = "Tienes un total de { "+miDB.comprobarTodosFavorito(miDB.NomUser)+" } de favoritos entre todas las categorias.";
        }

        /// <summary>
        /// Evento de carga del componente de diseño de la página, rellena los listbox en función de los datos que tenga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Se cargan los datos en los litsbox.
            lbPkmFav.ItemsSource = miDB.leerPkmsFavorito(miDB.NomUser);
            lbMovFav.ItemsSource = miDB.leerMovsFavorito(miDB.NomUser);
            lbTipFav.ItemsSource = miDB.leerTiposFavorito(miDB.NomUser);
        }
    }
}
