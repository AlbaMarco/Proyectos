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
    /// Lógica de interacción para ISCorrecto.xaml
    /// </summary>
    public partial class ISCorrecto : Page
    {
        private DB miDB;
        /// <summary>
        /// Constructor que se encarga de comprobar el nivel del usuario y mostrarle lo correcto, además de su nombre en la pantalla.
        /// </summary>
        /// <param name="db"></param>
        public ISCorrecto(DB db)
        {
            InitializeComponent();
            miDB = db;
            comprobarNivel();
            controlTab.miDB = miDB;
        }

        /// <summary>
        /// Comprueba el nivel de usuario y dependiendo de este, se podrá acceder a x contenido o y.
        /// </summary>
        public void comprobarNivel()
        {
            switch (miDB.nivelUserConectado)
            {
                case "1": // Básico
                    nivelUsers.Source = new BitmapImage(new Uri("../Resources/Tier1.png", UriKind.Relative));
                    nomUser.Content = miDB.NomUser;
                    visibilidadControl();
                    break;
                case "2": // Medio
                    nivelUsers.Source = new BitmapImage(new Uri("../Resources/Tier2.png", UriKind.Relative));
                    nomUser.Content = miDB.NomUser;
                    visibilidadControl();
                    break;
                case "3": // Alto
                    nivelUsers.Source = new BitmapImage(new Uri("../Resources/Tier3.png", UriKind.Relative));
                    nomUser.Content = miDB.NomUser;
                    visibilidadControl();
                    break;
                case "4": // Total
                    nivelUsers.Source = new BitmapImage(new Uri("../Resources/Tier4.png", UriKind.Relative));
                    nomUser.Content = miDB.NomUser;
                    visibilidadControl();
                    break;
            } // switch
        }

        /// <summary>
        /// Controla el nivel del usuario, para ver que se le muestra y que se le muestra según su nivel.
        /// Realmente no es necesario que se utilice el visible, ya que es su valor por defecto, pero es usado para una mayor aclaración.
        /// </summary>
        public void visibilidadControl()
        {
            switch (miDB.nivelUserConectado)
            {
                case "1": // Básico
                    controlTab.BusquedaPkm.Visibility = Visibility.Visible;
                    controlTab.Pokedex.Visibility = Visibility.Visible;
                    controlTab.Movimientos.Visibility = Visibility.Visible;
                    controlTab.Tipos.Visibility = Visibility.Hidden;
                    controlTab.Pokeball.Visibility = Visibility.Hidden;
                    controlTab.Bayas.Visibility = Visibility.Hidden;
                    controlTab.ItemsEstado.Visibility = Visibility.Hidden;
                    controlTab.ItemsEvolucion.Visibility = Visibility.Hidden;
                    controlTab.LamArceus.Visibility = Visibility.Hidden;
                    controlTab.Cartas.Visibility = Visibility.Hidden;
                    controlTab.Vitaminas.Visibility = Visibility.Hidden;

                    CrearEquipos.Visibility = Visibility.Hidden;
                    HacerEnfrentamientos.Visibility = Visibility.Hidden;
                    Ranking.Visibility = Visibility.Hidden;
                    break;
                case "2": // Medio
                    controlTab.BusquedaPkm.Visibility = Visibility.Visible;
                    controlTab.Pokedex.Visibility = Visibility.Visible;
                    controlTab.Movimientos.Visibility = Visibility.Visible;
                    controlTab.Tipos.Visibility = Visibility.Visible;
                    controlTab.Pokeball.Visibility = Visibility.Visible;
                    controlTab.Bayas.Visibility = Visibility.Hidden;
                    controlTab.ItemsEstado.Visibility = Visibility.Hidden;
                    controlTab.ItemsEvolucion.Visibility = Visibility.Hidden;
                    controlTab.LamArceus.Visibility = Visibility.Hidden;
                    controlTab.Cartas.Visibility = Visibility.Hidden;
                    controlTab.Vitaminas.Visibility = Visibility.Hidden;

                    CrearEquipos.Visibility = Visibility.Visible;
                    HacerEnfrentamientos.Visibility = Visibility.Hidden;
                    Ranking.Visibility = Visibility.Hidden;
                    break;
                case "3": // Alto
                    controlTab.BusquedaPkm.Visibility = Visibility.Visible;
                    controlTab.Pokedex.Visibility = Visibility.Visible;
                    controlTab.Movimientos.Visibility = Visibility.Visible;
                    controlTab.Tipos.Visibility = Visibility.Visible;
                    controlTab.Pokeball.Visibility = Visibility.Visible;
                    controlTab.Bayas.Visibility = Visibility.Visible;
                    controlTab.ItemsEstado.Visibility = Visibility.Visible;
                    controlTab.ItemsEvolucion.Visibility = Visibility.Visible;
                    controlTab.LamArceus.Visibility = Visibility.Hidden;
                    controlTab.Cartas.Visibility = Visibility.Hidden;
                    controlTab.Vitaminas.Visibility = Visibility.Hidden;

                    CrearEquipos.Visibility = Visibility.Visible;
                    HacerEnfrentamientos.Visibility = Visibility.Visible;
                    Ranking.Visibility = Visibility.Hidden;
                    break;
                case "4": // Total
                    controlTab.BusquedaPkm.Visibility = Visibility.Visible;
                    controlTab.Pokedex.Visibility = Visibility.Visible;
                    controlTab.Movimientos.Visibility = Visibility.Visible;
                    controlTab.Tipos.Visibility = Visibility.Visible;
                    controlTab.Pokeball.Visibility = Visibility.Visible;
                    controlTab.Bayas.Visibility = Visibility.Visible;
                    controlTab.ItemsEstado.Visibility = Visibility.Visible;
                    controlTab.ItemsEvolucion.Visibility = Visibility.Visible;
                    controlTab.LamArceus.Visibility = Visibility.Visible;
                    controlTab.Cartas.Visibility = Visibility.Visible;
                    controlTab.Vitaminas.Visibility = Visibility.Visible;

                    CrearEquipos.Visibility = Visibility.Visible;
                    HacerEnfrentamientos.Visibility = Visibility.Visible;
                    Ranking.Visibility = Visibility.Visible;
                    break;
            } // switch
        } // Visibilidad control.

        /// <summary>
        /// Método que se ejecutará cada vez que la página se abra. Al ser la que va después de inicio de seión, le quito la entrada a esa página para que no pueda retroceder a ella.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
        }

        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void SoporteTecnico_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SoporteTecnico());
        }

        private void CrearEquipos_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearEquipos(miDB));
        }

        private void VerFavoritos_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AccesoRegistrado.VerFavs(miDB));
        }
    }
}
