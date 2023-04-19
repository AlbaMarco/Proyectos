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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDI.Pags
{
    /// <summary>
    /// Lógica de interacción para Gratuito.xaml
    /// </summary>
    public partial class Gratuito : Page
    {
        private JsonDocument jsonPokemon;
        private string respuestaPokemon;
        /// <summary>
        /// Constructor.
        /// </summary>
        public Gratuito()
        {
            InitializeComponent();
        }

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
                if(respuestaPokemon != null && respuestaPokemon != "Not Found")
                {
                    jsonPokemon = JsonDocument.Parse(respuestaPokemon);
                } else
                {
                    MessageBox.Show("No se han encontrados datos por ese nombre.");
                }
                
            }
        }

        /// <summary>
        /// Acción del click del botón, necesaria para buscar el pokemon que se meta en el textBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnBusqueda_Click(object sender, RoutedEventArgs e)
        {
            string pokemon = txBoxNomPkm.Text;
            if(pokemon != string.Empty)
            {
                await PeticionPkm(pokemon.ToLower());
            }
            else
            {
                MessageBox.Show("No introdujo datos a buscar.");
            }
            
            if(jsonPokemon != null)
            {
                string pkm = jsonPokemon.RootElement.GetProperty("name").ToString();
                BitmapImage img = new BitmapImage(new Uri(jsonPokemon.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));

                // CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) Esto lo que hace es sacarme la primera letra en mayúscula.
                lbBusqueda.Items.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
            }

            txBoxNomPkm.Text = "";
            
        }

        /// <summary>
        /// Acción de cuando se selecciona y se hace doble click en cualquier pokemon de la lista.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lbBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            lbFormas.Items.Clear();
            tbTipo.Text = "";
            tbHabilidad.Text = "";

            string nom = lbBusqueda.SelectedItem.ToString();
            string[] contenido = nom.Split(' ');
            string nomPkm = contenido[6].ToLower();

            await PeticionPkm(nomPkm);

            tbId.Text = jsonPokemon.RootElement.GetProperty("id").ToString();
            tbNombre.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(jsonPokemon.RootElement.GetProperty("name").ToString());
            tbAltura.Text = jsonPokemon.RootElement.GetProperty("height").ToString();
            tbPeso.Text = jsonPokemon.RootElement.GetProperty("weight").ToString();

            // Para sacar todos los tipos que tiene el pokemon.
            for(int i = 0; i < jsonPokemon.RootElement.GetProperty("types").GetArrayLength(); i++)
            {
                int max = jsonPokemon.RootElement.GetProperty("types").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        tbTipo.Text += jsonPokemon.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        tbTipo.Text += jsonPokemon.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) tbTipo.Text += jsonPokemon.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString() + ".";
            }

            // Para sacar las habilidades que hay en ese pokemon.
            for (int i = 0; i < jsonPokemon.RootElement.GetProperty("abilities").GetArrayLength(); i++)
            {
                int max = jsonPokemon.RootElement.GetProperty("abilities").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        tbHabilidad.Text += jsonPokemon.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        tbHabilidad.Text += jsonPokemon.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) tbHabilidad.Text += jsonPokemon.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString() + ".";
            }

            // Añadir al listbox las imágenes.
            BitmapImage imgFront = new BitmapImage(new Uri(jsonPokemon.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            BitmapImage imgBack = new BitmapImage(new Uri(jsonPokemon.RootElement.GetProperty("sprites").GetProperty("back_default").ToString()));
            BitmapImage imgShinyFront = new BitmapImage(new Uri(jsonPokemon.RootElement.GetProperty("sprites").GetProperty("front_shiny").ToString()));
            BitmapImage imgShinyBack = new BitmapImage(new Uri(jsonPokemon.RootElement.GetProperty("sprites").GetProperty("back_shiny").ToString()));
            lbFormas.Items.Add(new { ImaFront = imgFront, ImaBack = imgBack, ImaShinyFront = imgShinyFront, ImaShinyBack = imgShinyBack });
        }
    }
}
