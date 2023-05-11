using AppDI.Recursos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDI.Pags.PanelAdmin.Admin
{
    /// <summary>
    /// Lógica de interacción para BanearPkm.xaml
    /// Ventana que se utiliza para poder banear a un pokemon ingresado en un textbox.
    /// Se utilizará para que ningún equipo lo elegir.
    /// </summary>
    public partial class BanearPkm : Page
    {
        /// <summary>
        /// Variable de tipo base de datos. Se va trasladando de ventana a ventana desde que se inicia sesión.
        /// </summary>
        private DB miDB;

        /// <summary>
        /// Constructor en el que se inicializan los componenetes, se obtiene una variable de tipo base de datos que se lleva desde que se incia sesión
        /// y se oculta el botón de banear y la etiqueta que dice cual fue el resultado.
        /// </summary>
        /// <param name="db"></param>
        public BanearPkm(DB db)
        {
            InitializeComponent();
            miDB = db;
            btnBan.Visibility = Visibility.Hidden;
            lblResultado.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Evento al cargar la página que evita que la navegación cargue ya que para el proceso.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }

        /// <summary>
        /// Se deberá de introducir un nombre o una ID en el textbox y pulsar el botón. 
        /// Hay una comprobación de que esté rellenado el campo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnBusqueda_Click(object sender, RoutedEventArgs e)
        {
            string pokemon = txBoxNomPkm.Text;

            if (pokemon != string.Empty)
            {
                await PeticionPkm(pokemon.ToLower());
            }
            else
            {
                MessageBox.Show("No introdujo datos a buscar.");
            }

            if (jsonPokemon != null)
            {
                string pkm = jsonPokemon.RootElement.GetProperty("name").ToString();
                BitmapImage img = new BitmapImage(new Uri(jsonPokemon.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));

                // CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) Esto lo que hace es sacarme la primera letra en mayúscula.
                lbBusqueda.Items.Add(new { Imagen = img, IdPkm = jsonPokemon.RootElement.GetProperty("id").ToString() + "  ", NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm)+" ."});
            }

            txBoxNomPkm.Text = "";
        }

        /// <summary>
        /// Acción que se llevará a cabo una vez se haga doble click en el list box, seleciconando un pokemon e insertándolo en las labels.
        /// Una vez que se haya insertado en las labels, se podrá pulsar el botón de banear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int indiceId1 = lbBusqueda.SelectedItem.ToString().IndexOf("IdPkm = ") + "IdPkm = ".Length;
            int indiceId2 = lbBusqueda.SelectedItem.ToString().IndexOf("  ");
            int caracteres = indiceId2 - indiceId1;
            string IdPkm = lbBusqueda.SelectedItem.ToString().Substring(indiceId1, caracteres);

            int indiceNom1 = lbBusqueda.SelectedItem.ToString().IndexOf("NomPkm = ") + "NomPkm = ".Length;
            int indiceNom2 = lbBusqueda.SelectedItem.ToString().IndexOf(" .");
            int caracteresNom = indiceNom2 - indiceNom1;
            string NomPkm = lbBusqueda.SelectedItem.ToString().Substring(indiceNom1, caracteresNom);

            lblIdPkm.Content = IdPkm;
            lblNomPkm.Content = NomPkm;

            btnBan.Visibility = Visibility.Visible;
            lblResultado.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Ventana de banear Pokemon, sirve para coger los datos asignados a las labels que es la ID y el Nombre del Pokémon.
        /// No hay comprobación de si están vacios porque el botón es inviisble hasta que seleccionan algún dato del ListBox de la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBan_Click(object sender, RoutedEventArgs e)
        {
            if (miDB.banearPkm(lblIdPkm.Content.ToString(), lblNomPkm.Content.ToString()) == 1)
            {
                lblResultado.Content = "Baneado correctamente.";
            } else { lblResultado.Content = "Hubo algún error."; }
        }

        /// <summary>
        /// Método para ver todos los pkms baneados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerBans_Click(object sender, RoutedEventArgs e)
        {
            listaDataGrid.ItemsSource = miDB.selectPkmBaneadosTodo().Tables[0].DefaultView;
        }

        private JsonDocument jsonPokemon;
        private string respuestaPokemon;

        /// <summary>
        /// Petición realizada para buscar los pokemons que se introduzcan por nombr --> https://pokeapi.co/api/v2/pokemon/{id or name}/
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        private async Task PeticionPkm(string nombre) // Admite id o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "pokemon/" + nombre + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaPokemon = await response.Content.ReadAsStringAsync();
                }

                if (respuestaPokemon != null && respuestaPokemon != "Not Found")
                {
                    jsonPokemon = JsonDocument.Parse(respuestaPokemon);
                }
                else
                {
                    MessageBox.Show("No se han encontrados datos por ese nombre.");
                    jsonPokemon = null;
                }

            }
        }

    } // Clase
}
