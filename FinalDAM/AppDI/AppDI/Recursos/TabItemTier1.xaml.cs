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
using System.IO;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using AppDI.Pags;

namespace AppDI.Recursos
{
    /// <summary>
    /// Lógica de interacción para TabItemTier1.xaml
    /// </summary>
    public partial class TabItemTier1 : UserControl
    {
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPokedex;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPokedex;

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPkm;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPkm;

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonForm;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaForm;

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonEvolu;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaEvolu;

        /// <summary>
        /// Propiedad para obtener el ID del pokemon.
        /// </summary>
        public string idPkm { get; set; }
        /// <summary>
        /// Propiedad para obtener el nombre del PKM.
        /// </summary>
        public string nombrePkm { get; set; }
        /// <summary>
        /// Variable de la base de datos.
        /// </summary>
        public DB miDB { get; set; }

        /// <summary>
        /// Variable de la aplicación princpal, para comunicar el hilo.
        /// </summary>
        App app = (App)Application.Current;
        /// <summary>
        /// Constructor donde se inicializan los componenetes de este User Control y se ejecuta los eventos de actualización propios.
        /// </summary>
        public TabItemTier1()
        {
            InitializeComponent();
            evoluciones.IsEnabled = false;
            app.Actualizar += App_Actualizar;
            app.ActualizarRoAzAm += App_ActualizarRoAzAm;
            app.ActualizarOroPlaCri += App_ActualizarOroPlaCri;
            app.ActualizarRubZafEsm += App_ActualizarRubZafEsm;
            app.ActualizarDiaPer += App_ActualizarDiaPer;
            app.ActualizarPlatino += App_ActualizarPlatino;
            app.ActualizarHerSoul += App_ActualizarHerSoul;
            app.ActualizarBlaNeg += App_ActualizarBlaNeg;
            app.ActualizarBlaNeg2 += App_ActualizarBlaNeg2;

        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex Blanco y Negro 2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarBlaNeg2(object sender, int e)
        {
            progresoBlaNeg2.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pkedex Blanco y negro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarBlaNeg(object sender, int e)
        {
            progresoBlaNeg.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex de Oro Hearthgold y Plata Soulsilver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarHerSoul(object sender, int e)
        {
            progresoHearSoul.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex de Pkm Platino.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarPlatino(object sender, int e)
        {
            progresoPlatino.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex de Diamante y Perla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarDiaPer(object sender, int e)
        {
            progresoDiaPer.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex de Rubi, Zafiro y Esmeralda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarRubZafEsm(object sender, int e)
        {
            progresoRuZaEsm.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex Oro, Plata y Cristal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarOroPlaCri(object sender, int e)
        {
            progresoOrPlCr.Value = e;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedez Rojo, Azul y Amarillo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ActualizarRoAzAm(object sender, int progreso)
        {
            progresoRoAzAm.Value = progreso;
        }

        /// <summary>
        /// Evento de actualización de la barra de progresión de Pokedex Nacional.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Actualizar(object sender, int progreso)
        {
            // Actualizar el valor de la barra de progreso
            progresoNacional.Value = progreso;
        }

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPkmBusqueda;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPkmBusqueda;

        /// <summary>
        /// Petición realizada para buscar los pokemons que se introduzcan por nombr --> https://pokeapi.co/api/v2/pokemon/{id or name}/
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        private async Task PeticionPkmBus(string nombre) // Admite id o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "pokemon/" + nombre + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaPkmBusqueda = await response.Content.ReadAsStringAsync();
                }

                if (respuestaPkmBusqueda != null && respuestaPkmBusqueda != "Not Found")
                {
                    jsonPkmBusqueda = JsonDocument.Parse(respuestaPkmBusqueda);
                }
                else
                {
                    MessageBox.Show("No se han encontrados datos por ese nombre.");
                    jsonPkmBusqueda = null;
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

            if (pokemon != string.Empty)
            {
                await PeticionPkmBus(pokemon.ToLower());
            }
            else
            {
                MessageBox.Show("No introdujo datos a buscar.");
            }

            if (jsonPkmBusqueda != null)
            {
                string pkm = jsonPkmBusqueda.RootElement.GetProperty("name").ToString();
                BitmapImage img = new BitmapImage(new Uri(jsonPkmBusqueda.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));

                // CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) Esto lo que hace es sacarme la primera letra en mayúscula.
                lbBusqueda.Items.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
            }

            txBoxNomPkm.Text = "";
        }

        /// <summary>
        /// Evento de acción asincrono para obtener el pokemon que se haya seleccionado en el List Box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lbBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            lbFormas.Items.Clear();
            tbTipo.Text = "";
            tbHabilidad.Text = "";
            lbMovimientos.Items.Clear();

            string nom = lbBusqueda.SelectedItem.ToString();
            string[] contenido = nom.Split(' ');
            string nomPkm = contenido[6].ToLower();

            await PeticionPkmBus(nomPkm);

            tbId.Text = jsonPkmBusqueda.RootElement.GetProperty("id").ToString();
            tbNombre.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(jsonPkmBusqueda.RootElement.GetProperty("name").ToString());
            tbAltura.Text = jsonPkmBusqueda.RootElement.GetProperty("height").ToString();
            tbPeso.Text = jsonPkmBusqueda.RootElement.GetProperty("weight").ToString();

            // Para sacar todos los tipos que tiene el pokemon.
            for (int i = 0; i < jsonPkmBusqueda.RootElement.GetProperty("types").GetArrayLength(); i++)
            {
                int max = jsonPkmBusqueda.RootElement.GetProperty("types").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        tbTipo.Text += jsonPkmBusqueda.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        tbTipo.Text += jsonPkmBusqueda.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) tbTipo.Text += jsonPkmBusqueda.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString() + ".";
            }

            // Para sacar las habilidades que hay en ese pokemon.
            for (int i = 0; i < jsonPkmBusqueda.RootElement.GetProperty("abilities").GetArrayLength(); i++)
            {
                int max = jsonPkmBusqueda.RootElement.GetProperty("abilities").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        tbHabilidad.Text += jsonPkmBusqueda.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        tbHabilidad.Text += jsonPkmBusqueda.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) tbHabilidad.Text += jsonPkmBusqueda.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString() + ".";
            }

            // Para sacar los movimientos que hay en ese pokemon.
            for (int i = 0; i < jsonPkmBusqueda.RootElement.GetProperty("moves").GetArrayLength(); i++)
            {
                lbMovimientos.Items.Add(jsonPkmBusqueda.RootElement.GetProperty("moves")[i].GetProperty("move").GetProperty("name").ToString());
            }

            // Añadir al listbox las imágenes.
            BitmapImage imgFront = new BitmapImage(new Uri(jsonPkmBusqueda.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            BitmapImage imgBack = new BitmapImage(new Uri(jsonPkmBusqueda.RootElement.GetProperty("sprites").GetProperty("back_default").ToString()));
            BitmapImage imgShinyFront = new BitmapImage(new Uri(jsonPkmBusqueda.RootElement.GetProperty("sprites").GetProperty("front_shiny").ToString()));
            BitmapImage imgShinyBack = new BitmapImage(new Uri(jsonPkmBusqueda.RootElement.GetProperty("sprites").GetProperty("back_shiny").ToString()));
            lbFormas.Items.Add(new { ImaFront = imgFront, ImaBack = imgBack, ImaShinyFront = imgShinyFront, ImaShinyBack = imgShinyBack });

            if(miDB.comprobarFavorito(miDB.NomUser, tbNombre.Text) == 0)
            {
                imgBtnFav.Source = new BitmapImage(new Uri("/Resources/FavVacio.png", UriKind.Relative));
                btnFavPokemon.IsEnabled = true;
            } else
            {
                imgBtnFav.Source = new BitmapImage(new Uri("/Resources/FavLleno.png", UriKind.Relative));
                btnFavPokemon.IsEnabled = false;
            }
        }

        /// <summary>
        /// Acción para añadir a favoritos el pokemon búscado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFavPokemon_Click(object sender, RoutedEventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            BitmapImage imgFront = new BitmapImage(new Uri(jsonPkmBusqueda.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            var encoder = new PngBitmapEncoder(); // Utilizo un BitMapImage para obtenerlo de la API.
            encoder.Frames.Add(BitmapFrame.Create(imgFront));
            encoder.Save(ms);
            byte[] bytesImg = ms.ToArray();

            string nombrePkm = tbNombre.Text;
            if(miDB.añadirFavoritos(miDB.NomUser, nombrePkm, bytesImg, "1") == 1)
            {
                imgBtnFav.Source = new BitmapImage(new Uri("/Resources/FavLleno.png", UriKind.Relative));
                btnFavPokemon.IsEnabled = false;
            }
        }

        /// <summary>
        /// Peticiones para la consulta de Pokedex. Se usa el número de pokedex.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private async Task PeticionPokedex(string num) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "pokedex/" + num + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaPokedex = await response.Content.ReadAsStringAsync();
                }

                jsonPokedex = JsonDocument.Parse(respuestaPokedex);
            }
        }

        /// <summary>
        /// Peticion para un sólo pokemon, se le pasa por parámetro el nombre del pokemon y te devolverá su información.
        /// </summary>
        /// <param name="pkm"></param>
        /// <returns></returns>
        private async Task PeticionPokemon(string pkm) // Admite idPkm o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "pokemon/" + pkm + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaPkm = await response.Content.ReadAsStringAsync();
                }

                jsonPkm = JsonDocument.Parse(respuestaPkm);
            }
        }

        /// <summary>
        /// Petición para conseguir la imagen de un pokemon. Se mostrará todas las formas que este tenga.
        /// </summary>
        /// <param name="pkm"></param>
        /// <returns></returns>
        private async Task PeticionFotoPkm(string pkm) // Admite idPkm o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "pokemon-form/" + pkm + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaForm = await response.Content.ReadAsStringAsync();
                }

                jsonForm = JsonDocument.Parse(respuestaForm);
            }
        }

        /// <summary>
        /// Petición para saber la url de la cadena evolutiva de un pokemon.
        /// </summary>
        /// <param name="idPkm"></param>
        /// <returns></returns>
        private async Task PeticionEvoluciones(string idPkm) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "pokemon-species/" + idPkm + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaEvolu = await response.Content.ReadAsStringAsync();
                }

                jsonEvolu = JsonDocument.Parse(respuestaEvolu);
            }
        } // PeticionesEvoluciones.

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonCadEvo;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaCadEvo;

        /// <summary>
        /// Petición para saber la cadena evolutiva completa ddel pokemon pasado en el método anterior.
        /// </summary>
        /// <param name="urlEvolution"></param>
        /// <returns></returns>
        private async Task PeticionCadEvolu(string urlEvolution)
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = urlEvolution;

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaCadEvo = await response.Content.ReadAsStringAsync();
                }

                jsonCadEvo = JsonDocument.Parse(respuestaCadEvo);
            }
        } // PeticionesEvoluciones.

        /// <summary>
        /// Hace peticiones para poder rellenar el listbox de la pantalla izquierda de la Pokeddex.
        /// </summary>
        /// <param name="numGen"></param>
        private async void rellenarPokedex(string numGen)
        {
            BusquedaAPI.ItemsSource = null;
            BusquedaAPI.Items.Clear();
            evoluciones.IsEnabled = false;

            await PeticionPokedex(numGen);

            int num = jsonPokedex.RootElement.GetProperty("pokemon_entries").GetArrayLength();
            for (int i = 0; i < jsonPokedex.RootElement.GetProperty("pokemon_entries").GetArrayLength(); i++)
            {
                // Para que la primera letra sea mayúscula.
                string pkm = jsonPokedex.RootElement.GetProperty("pokemon_entries")[i].GetProperty("pokemon_species").GetProperty("name").ToString();
                string idPkm = jsonPokedex.RootElement.GetProperty("pokemon_entries")[i].GetProperty("pokemon_species").GetProperty("url").ToString().Substring(42); // Número donde coge la ID.
                await PeticionFotoPkm(idPkm);

                BitmapImage img = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));

                // CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) Esto lo que hace es sacarme la primera letra en mayúscula.
                BusquedaAPI.Items.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
            }
        }

        /// <summary>
        /// Comprueba el botón pulsado, ya que cada botón es una consulta de Pokedex distinta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            BusquedaAPI.ItemsSource = null;
            if (nacional.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxNacional;
            }
            else if (roAzAm.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxRoAzAm;
            }
            else if (oroPlaCris.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxOroPlaCri;
            }
            else if (rubSafEm.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxRubZafEsm;
            }
            else if (diaPer.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxDiaPer;
            }
            else if (platino.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxPlatino;
            }
            else if (heartSoul.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxHerSoul;
            }
            else if (blanNeg.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxBlaNeg;
            }
            else if (blanNeg2.IsChecked == true)
            {
                BusquedaAPI.ItemsSource = app.listaPkxBlaNeg2;
            }// Fin else if.
        } // Rdbtn checkeado.

        /// <summary>
        /// En caso de que se le haga doble click al listbox de la izquierda, se abrirá a la derecha los datos del pokemon seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BusquedaAPI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BusquedaAPI.SelectedIndex != -1)
            {
                PkmSelecc.Items.Clear();
                string nomLB = BusquedaAPI.SelectedItem.ToString();
                string[] contenido = nomLB.Split(' ');
                string nomPkm = contenido[6].ToLower(); // Me coge del elemento únicamente el nombre, que es lo que me interesa para hacer la búsqueda del pkm y sacar su información.

                // Para sacar su ID y poder hacer la consulta a su cadena evolutiva, primero haremos la búsqueda pkm y se mostrará su información a la parte izq.
                await PeticionPokemon(nomPkm);

                idPkm = jsonPkm.RootElement.GetProperty("id").ToString();
                nombrePkm = jsonPkm.RootElement.GetProperty("name").ToString();

                PkmSelecc.Items.Add(new { TituloPkm = "Altura:", NomPkm = jsonPkm.RootElement.GetProperty("height").ToString() + "cm" });
                PkmSelecc.Items.Add(new { TituloPkm = "Peso:", NomPkm = jsonPkm.RootElement.GetProperty("weight").ToString() + "kg" });
               

                PkmSelecc.Items.Add(new { TituloPkm = "Tipo/s:", NomPkm = jsonPkm.RootElement.GetProperty("types").GetArrayLength().ToString() });
                for (int i = 0; i < jsonPkm.RootElement.GetProperty("types").GetArrayLength(); i++)
                {
                    PkmSelecc.Items.Add(new { TituloPkm = "  " + jsonPkm.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString(), NomPkm = "" });
                }

                PkmSelecc.Items.Add(new {TituloPkm = "Habilidad/es", NomPkm = jsonPkm.RootElement.GetProperty("abilities").GetArrayLength().ToString() });
                for(int i = 0; i < jsonPkm.RootElement.GetProperty("abilities").GetArrayLength(); i++)
                {
                    PkmSelecc.Items.Add(new { TituloPkm = "  " + jsonPkm.RootElement.GetProperty("abilities")[i].GetProperty("ability").GetProperty("name").ToString(), NomPkm = "" });
                }

                evoluciones.IsEnabled = true;
            }


        } // Búsqueda doble click.

        /// <summary>
        /// Comprueba si el botón para ver las evoluciones está pulsado.
        /// En caso de que sí lo esté, se mostrará una ventana personalizada con las evoluciones disponibles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void evoluciones_Checked(object sender, RoutedEventArgs e)
        {
            if (evoluciones.IsChecked == true)
            {
                // DE LA BÚSQUEDA DE POKEDEX, EL URL DEL POKEMON_SPECIES
                // Te da la URL de la API de la cadena evolutiva.
                await PeticionEvoluciones(idPkm);

                string urlCad = jsonEvolu.RootElement.GetProperty("evolution_chain").GetProperty("url").ToString().Substring(26);
                await PeticionCadEvolu(urlCad);


                if (jsonCadEvo.RootElement.GetProperty("chain").GetProperty("evolves_to").GetArrayLength() == 0)
                {
                    string primEvo = jsonCadEvo.RootElement.GetProperty("chain").GetProperty("species").GetProperty("name").ToString();
                    mostrarEvoDe1(primEvo);
                    evoluciones.IsChecked = false;
                }

                if (jsonCadEvo.RootElement.GetProperty("chain").GetProperty("evolves_to").GetArrayLength() == 1)
                {
                    string primEvo = jsonCadEvo.RootElement.GetProperty("chain").GetProperty("species").GetProperty("name").ToString();
                    string segunEvo = jsonCadEvo.RootElement.GetProperty("chain").GetProperty("evolves_to")[0].GetProperty("species").GetProperty("name").ToString();
                    if (jsonCadEvo.RootElement.GetProperty("chain").GetProperty("evolves_to")[0].GetProperty("evolves_to").GetArrayLength() == 1)
                    {
                        string tercerEvo = jsonCadEvo.RootElement.GetProperty("chain").GetProperty("evolves_to")[0].GetProperty("evolves_to")[0].GetProperty("species").GetProperty("name").ToString();
                        mostrarEvoluciones(primEvo, segunEvo, tercerEvo);
                        evoluciones.IsChecked = false;
                    }
                    else
                    {
                        // Entrará si tiene sólo dos evoluciones.
                        mostrarEvoDe2(primEvo, segunEvo);
                        evoluciones.IsChecked = false;
                    }
                } // Final bucles IF.
            } // If
        } // evoluciones ToggleBotton.

        /// <summary>
        /// Ventana emergente custom mencionada en el método ddel Toggle Button.
        /// Esta es si tiene tres evoluciones.
        /// </summary>
        /// <param name="primEvo"></param>
        /// <param name="segunEvo"></param>
        /// <param name="terceEvo"></param>
        private async void mostrarEvoluciones(string primEvo, string segunEvo, string terceEvo)
        {
            Window custom = new Window();
            custom.Title = "Evoluciones de " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(primEvo);
            custom.Width = 400;
            custom.Height = 100;
            custom.Icon = new BitmapImage(new Uri("../../../Resources/PbAbierta.png", UriKind.Relative));

            FontFamily fuente = new FontFamily(new Uri("pack://application:,,,/"), "./Fuentes/#Pocket Monk");

            Image img1 = new Image();
            Image img2 = new Image();
            Image img3 = new Image();

            jsonForm = null;

            await PeticionFotoPkm(primEvo);

            img1.Source = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            img1.Width = 50;
            img1.Height = 50;
            img1.VerticalAlignment = VerticalAlignment.Center;
            img1.HorizontalAlignment = HorizontalAlignment.Left;

            jsonForm = null;

            await PeticionFotoPkm(segunEvo);
            img2.Source = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            img2.Width = 50;
            img2.Height = 50;
            img2.VerticalAlignment = VerticalAlignment.Center;
            img2.HorizontalAlignment = HorizontalAlignment.Left;

            jsonForm = null;

            await PeticionFotoPkm(terceEvo);
            img3.Source = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            img3.Width = 50;
            img3.Height = 50;
            img3.VerticalAlignment = VerticalAlignment.Center;
            img3.HorizontalAlignment = HorizontalAlignment.Left;

            Thickness margin = new Thickness(5, 0, 5, 0);
            TextBlock text1 = new TextBlock();
            text1.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(primEvo);
            text1.VerticalAlignment = VerticalAlignment.Center;
            text1.HorizontalAlignment = HorizontalAlignment.Center;
            text1.Margin = margin;
            text1.FontFamily = fuente;

            TextBlock text2 = new TextBlock();
            text2.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(segunEvo);
            text2.VerticalAlignment = VerticalAlignment.Center;
            text2.HorizontalAlignment = HorizontalAlignment.Center;
            text2.Margin = margin;
            text2.FontFamily = fuente;

            TextBlock text3 = new TextBlock();
            text3.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(terceEvo);
            text3.VerticalAlignment = VerticalAlignment.Center;
            text3.HorizontalAlignment = HorizontalAlignment.Center;
            text3.Margin = margin;
            text3.FontFamily = fuente;


            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;


            stackPanel.Children.Add(img1);
            stackPanel.Children.Add(text1);
            stackPanel.Children.Add(img2);
            stackPanel.Children.Add(text2);
            stackPanel.Children.Add(img3);
            stackPanel.Children.Add(text3);


            custom.Content = stackPanel;
            custom.ShowDialog();
        } // Para 3 evoluciones.

        /// <summary>
        /// Ventana emergente custom mencionada en el método ddel Toggle Button.
        /// Esta es si tiene dos evoluciones.
        /// </summary>
        /// <param name="primEvo"></param>
        /// <param name="segunEvo"></param>
        private async void mostrarEvoDe2(string primEvo, string segunEvo)
        {
            Window custom = new Window();
            custom.Title = "Evoluciones de " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(primEvo);
            custom.Width = 400;
            custom.Height = 100;
            custom.Icon = new BitmapImage(new Uri("../../../Resources/PbAbierta.png", UriKind.Relative));

            FontFamily fuente = new FontFamily(new Uri("pack://application:,,,/"), "./Fuentes/#Pocket Monk");

            Image img1 = new Image();
            Image img2 = new Image();
            Image img3 = new Image();

            jsonForm = null;

            await PeticionFotoPkm(primEvo);

            img1.Source = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            img1.Width = 50;
            img1.Height = 50;
            img1.VerticalAlignment = VerticalAlignment.Center;
            img1.HorizontalAlignment = HorizontalAlignment.Left;

            jsonForm = null;

            await PeticionFotoPkm(segunEvo);
            img2.Source = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            img2.Width = 50;
            img2.Height = 50;
            img2.VerticalAlignment = VerticalAlignment.Center;
            img2.HorizontalAlignment = HorizontalAlignment.Left;

            Thickness margin = new Thickness(5, 0, 10, 0);
            TextBlock text1 = new TextBlock();
            text1.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(primEvo);
            text1.VerticalAlignment = VerticalAlignment.Center;
            text1.HorizontalAlignment = HorizontalAlignment.Center;
            text1.Margin = margin;
            text1.FontFamily = fuente;

            TextBlock text2 = new TextBlock();
            text2.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(segunEvo);
            text2.VerticalAlignment = VerticalAlignment.Center;
            text2.HorizontalAlignment = HorizontalAlignment.Center;
            text2.Margin = margin;
            text2.FontFamily = fuente;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;


            stackPanel.Children.Add(img1);
            stackPanel.Children.Add(text1);
            stackPanel.Children.Add(img2);
            stackPanel.Children.Add(text2);


            custom.Content = stackPanel;
            custom.ShowDialog();
        } // Para 2 evoluciones.

        /// <summary>
        /// Ventana emergente custom mencionada en el método ddel Toggle Button.
        /// Esta es si tiene una evolucion.
        /// </summary>
        /// <param name="primEvo"></param>
        private async void mostrarEvoDe1(string primEvo)
        {
            Window custom = new Window();
            custom.Title = "Evoluciones de " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(primEvo);
            custom.Width = 400;
            custom.Height = 100;
            custom.Icon = new BitmapImage(new Uri("../../../Resources/PbAbierta.png", UriKind.Relative));

            FontFamily fuente = new FontFamily(new Uri("pack://application:,,,/"), "./Fuentes/#Pocket Monk");

            Image img1 = new Image();
            Image img2 = new Image();
            Image img3 = new Image();

            jsonForm = null;

            await PeticionFotoPkm(primEvo);

            img1.Source = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
            img1.Width = 50;
            img1.Height = 50;
            img1.VerticalAlignment = VerticalAlignment.Center;
            img1.HorizontalAlignment = HorizontalAlignment.Left;

            Thickness margin = new Thickness(5, 0, 10, 0);
            TextBlock text1 = new TextBlock();
            text1.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(primEvo);
            text1.VerticalAlignment = VerticalAlignment.Center;
            text1.HorizontalAlignment = HorizontalAlignment.Center;
            text1.Margin = margin;
            text1.FontFamily = fuente;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;


            stackPanel.Children.Add(img1);
            stackPanel.Children.Add(text1);


            custom.Content = stackPanel;
            custom.ShowDialog();
        } // Para 1 evolucion.

        // FIN POKEDEX



        // MOVIMIENTOS POKEMON
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonMovsGen;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaMovsGen;
        /// <summary>
        /// Petición de los movimientos por generación añadidos al juego.
        /// </summary>
        /// <param name="gen"></param>
        /// <returns></returns>
        private async Task PeticionMovsGen(string gen) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                if (lbMovs != null) { lbMovs.Items.Clear(); }
                string consulta = "generation/" + gen + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaMovsGen = await response.Content.ReadAsStringAsync();
                }

                jsonMovsGen = JsonDocument.Parse(respuestaMovsGen);

                if (jsonMovsGen.RootElement.GetProperty("moves").GetArrayLength() == 0)
                {
                    lbMovs.Items.Add("No hay ningún movimiento nuevo en esta generación");
                }
                else
                {
                    for (int i = 0; i < jsonMovsGen.RootElement.GetProperty("moves").GetArrayLength(); i++)
                    {
                        lbMovs.Items.Add(jsonMovsGen.RootElement.GetProperty("moves")[i].GetProperty("name").ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonMov;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaMov;
        /// <summary>
        /// Petición para obtener información sobre el movimiento seleccionado.
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        private async Task PeticionMov(string consulta) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaMov = await response.Content.ReadAsStringAsync();
                }

                jsonMov = JsonDocument.Parse(respuestaMov);
            }
        }

        /// <summary>
        /// Método para se cambie el movimiento seleccionado y cargue sus datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void selecGen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await PeticionMovsGen((selecGen.SelectedIndex + 1).ToString());
        }

        /// <summary>
        /// Método por cada vez que se hace doble click, llamando a la información del movimiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lbMovs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await PeticionMov(jsonMovsGen.RootElement.GetProperty("moves")[lbMovs.SelectedIndex].GetProperty("url").ToString().Substring(26));
            txtAcierto.Text = jsonMov.RootElement.GetProperty("accuracy").ToString();
            if (jsonMov.RootElement.GetProperty("damage_class").GetProperty("name").ToString() == "physical") txtTipoDanio.Text = "físico";
            else txtTipoDanio.Text = "especial";

            txtPoder.Text = jsonMov.RootElement.GetProperty("power").ToString();
            txtPP.Text = jsonMov.RootElement.GetProperty("pp").ToString();
            txtPrio.Text = jsonMov.RootElement.GetProperty("priority").ToString();
            txtTipo.Text = jsonMov.RootElement.GetProperty("type").GetProperty("name").ToString();

            if (miDB.comprobarFavorito(miDB.NomUser, jsonMov.RootElement.GetProperty("name").ToString()) == 0)
            {
                imgBtnFavMov.Source = new BitmapImage(new Uri("/Resources/FavVacio.png", UriKind.Relative));
                btnFavMov.IsEnabled = true;
            }
            else
            {
                imgBtnFavMov.Source = new BitmapImage(new Uri("/Resources/FavLleno.png", UriKind.Relative));
                btnFavMov.IsEnabled = false;
            }
        }

        /// <summary>
        /// Evento que se lleva a cabo cuando se realiza Click al añadir a favoritos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFavMov_Click(object sender, RoutedEventArgs e)
        {
            string nombreMov = jsonMov.RootElement.GetProperty("name").ToString();
            if (miDB.añadirFavoritosNoImg(miDB.NomUser, nombreMov, "2") == 1)
            {
                imgBtnFavMov.Source = new BitmapImage(new Uri("/Resources/FavLleno.png", UriKind.Relative));
                btnFavMov.IsEnabled = false;
            }
        }

        // FIN MOVIMIENTOS



        // TIPOS POKEMON
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonTipGen;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaTipGen;
        /// <summary>
        /// Método de llamda a la API para obtener la generación
        /// </summary>
        /// <param name="gen"></param>
        /// <returns></returns>
        private async Task PeticionTipGen(string gen) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                if (lbTipos != null) { lbTipos.Items.Clear(); }
                string consulta = "generation/" + gen + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaTipGen = await response.Content.ReadAsStringAsync();
                }

                jsonTipGen = JsonDocument.Parse(respuestaTipGen);

                if(jsonTipGen.RootElement.GetProperty("types").GetArrayLength() == 0)
                {
                    lbTipos.Items.Add("No hay ningún tipo nuevo en esta generación");
                } else
                {
                    for (int i = 0; i < jsonTipGen.RootElement.GetProperty("types").GetArrayLength(); i++)
                    {
                        lbTipos.Items.Add(jsonTipGen.RootElement.GetProperty("types")[i].GetProperty("name").ToString());
                    }
                }
            } // Using
        } // Peticion de la generación tipos.

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonTip;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaTip;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        private async Task PeticionTip(string consulta) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaTip = await response.Content.ReadAsStringAsync();
                }

                jsonTip = JsonDocument.Parse(respuestaTip);
            }
        } // PeticionTip

        /// <summary>
        /// Cuando la selección de mi objeto cambie, se hará una petición nueva.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void selecGen_Types_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await PeticionTipGen((selecGen_Types.SelectedIndex + 1).ToString());
        }

        /// <summary>
        /// Acción de doble click al list box de los tipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lbTipos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await PeticionTip(jsonTipGen.RootElement.GetProperty("types")[lbTipos.SelectedIndex].GetProperty("url").ToString().Substring(26));
            // Un for por cada txt porque puede haber un tipo, dos, tres, todos que sean eficaz, no y demás.
            txtDoubleFrom.Text = "";
            txtHalfFrom.Text = "";
            txtDoubleTo.Text = "";
            txtHalfTo.Text = "";
            txtNoDanioFrom.Text = "";
            txtNoDanioTo.Text = "";

            for (int i = 0; i < jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_from").GetArrayLength(); i++)
            {
                int max = jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_from").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        txtDoubleFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_from")[i].GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        txtDoubleFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_from")[i].GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) txtDoubleFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_from")[i].GetProperty("name").ToString() + ".";
            } // Double damage from.

            for (int i = 0; i < jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_from").GetArrayLength(); i++)
            {
                int max = jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_from").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        txtHalfFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_from")[i].GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        txtHalfFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_from")[i].GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) txtHalfFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_from")[i].GetProperty("name").ToString() + ".";
            } // Half damage from.

            for (int i = 0; i < jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_to").GetArrayLength(); i++)
            {
                int max = jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_to").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        txtDoubleTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_to")[i].GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        txtDoubleTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_to")[i].GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) txtDoubleTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("double_damage_to")[i].GetProperty("name").ToString() + ".";
            } // Doble damage to.

            for (int i = 0; i < jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_to").GetArrayLength(); i++)
            {
                int max = jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_to").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        txtHalfTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_to")[i].GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        txtHalfTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_to")[i].GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) txtHalfTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("half_damage_to")[i].GetProperty("name").ToString() + ".";
            } // Half damage to.

            for (int i = 0; i < jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_from").GetArrayLength(); i++)
            {
                int max = jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_from").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        txtNoDanioFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_from")[i].GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        txtNoDanioFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_from")[i].GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) txtNoDanioFrom.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_from")[i].GetProperty("name").ToString() + ".";
            } // No damage from.

            for (int i = 0; i < jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_to").GetArrayLength(); i++)
            {
                int max = jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_to").GetArrayLength();
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        txtNoDanioTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_to")[i].GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        txtNoDanioTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_to")[i].GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1) txtNoDanioTo.Text += jsonTip.RootElement.GetProperty("damage_relations").GetProperty("no_damage_to")[i].GetProperty("name").ToString() + ".";
            } // No damage to.

            // Comprobación de si está o no en favoritos y dependiendo de si está o no se pone un icono u otro.
            if (miDB.comprobarFavorito(miDB.NomUser, jsonTip.RootElement.GetProperty("name").ToString()) == 0)
            {
                imgBtnFavTipo.Source = new BitmapImage(new Uri("/Resources/FavVacio.png", UriKind.Relative));
                btnFavTipo.IsEnabled = true;
            }
            else
            {
                imgBtnFavTipo.Source = new BitmapImage(new Uri("/Resources/FavLleno.png", UriKind.Relative));
                btnFavTipo.IsEnabled = false;
            }
        } // LbTipos

        /// <summary>
        /// Evento que se lleva a cabo cuando se le da click al botón de favoritos de los tipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFavTipo_Click(object sender, RoutedEventArgs e)
        {
            string nombreTipo = jsonTip.RootElement.GetProperty("name").ToString();
            if (miDB.añadirFavoritosNoImg(miDB.NomUser, nombreTipo, "3") == 1)
            {
                imgBtnFavTipo.Source = new BitmapImage(new Uri("/Resources/FavLleno.png", UriKind.Relative));
                btnFavTipo.IsEnabled = false;
            }
        }

        // FIN ETIQUETA TIPOS.


        // INICIO POKEBOLAS
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPkballCat;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPkballCat;

        /// <summary>
        /// Llamada a la API para obtener todos los tipos de Pokaballs disponibles.
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        private async Task PeticionPkballCat(string cat) // Admite ID y nombre cat
        {
            // 34 Normales, 33 Especiales, 39 Bonguri
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                string consulta = "item-category/" + cat + "/";

                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaPkballCat = await response.Content.ReadAsStringAsync();
                }

                jsonPkballCat = JsonDocument.Parse(respuestaPkballCat);


                if (cat == "33")
                {
                    totalEspeciales.Content = "Total: " + jsonPkballCat.RootElement.GetProperty("items").GetArrayLength().ToString();
                }
                if (cat == "34")
                {
                    totalNormales.Content = "Total: " + jsonPkballCat.RootElement.GetProperty("items").GetArrayLength().ToString();
                }
                if (cat == "39")
                {
                    totalBonguri.Content = "Total: " + jsonPkballCat.RootElement.GetProperty("items").GetArrayLength().ToString();
                }

                for (int i = 0; i < jsonPkballCat.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    PeticionPball(jsonPkballCat.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26), cat);
                }

            }
        } // Peticion de las categorias de las pokebollas.

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPball;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPball;
        /// <summary>
        /// Lista para el listbox de pokebolas normales.
        /// </summary>
        private List<object> listaNorm = new List<object>();
        /// <summary>
        /// Lista para el litbos de Pokebolas especiales.
        /// </summary>
        private List<object> listaEspe = new List<object>();
        /// <summary>
        /// Lista para el listbox de Pokebolas hecas por Bonguri.
        /// </summary>
        private List<object> listaBongu = new List<object>();

        /// <summary>
        /// Llamada a la API en la que se le pasa por parámetro la consulta y la categoria de la pokeball.
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        private async Task PeticionPball(string consulta, string cat) // Admite numGeneracion o nombre.
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaPball = await response.Content.ReadAsStringAsync();
                }

                jsonPball = JsonDocument.Parse(respuestaPball);
            }

            if (cat == "34")
            {
                BitmapImage img = new BitmapImage(new Uri(jsonPball.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                string nombre = jsonPball.RootElement.GetProperty("names")[6].GetProperty("name").ToString(); // Al ser el idioma "7" y en array es uno menos.
                string desc = jsonPball.RootElement.GetProperty("flavor_text_entries")[0].GetProperty("text").ToString();

                listaNorm.Add(new { Imagen = img, NomPkball = nombre, Descripcion = desc });
            }

            if (cat == "33")
            {
                BitmapImage img = new BitmapImage(new Uri(jsonPball.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                string nombre = jsonPball.RootElement.GetProperty("names")[6].GetProperty("name").ToString(); // Al ser el idioma "7" y en array es uno menos.
                string desc = jsonPball.RootElement.GetProperty("flavor_text_entries")[0].GetProperty("text").ToString();

                listaEspe.Add(new { Imagen = img, NomPkball = nombre, Descripcion = desc });
            }

            if (cat == "39")
            {
                BitmapImage img = new BitmapImage(new Uri(jsonPball.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                string nombre = jsonPball.RootElement.GetProperty("names")[6].GetProperty("name").ToString(); // Al ser el idioma "7" y en array es uno menos.
                string desc = jsonPball.RootElement.GetProperty("flavor_text_entries")[0].GetProperty("text").ToString();

                listaBongu.Add(new { Imagen = img, NomPkball = nombre, Descripcion = desc });
            }
        } // PeticionPballNormales

        /// <summary>
        /// Cada vez que se cargue el item de las Pokeballs, se rellenará los list box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            // 34 Normales, 33 Especiales, 39 Bonguri
            await PeticionPkballCat("34");
            await PeticionPkballCat("33");
            await PeticionPkballCat("39");

            PkbolNormal.ItemsSource = listaNorm;
            PkbolEspecial.ItemsSource = listaEspe;
            PkbolBonguri.ItemsSource = listaBongu;

            if (PkbolNormal.SelectedIndex == 0)
            {
                atrasPrim.IsEnabled = false;
                princPrim.IsEnabled = false;
            }

            if (PkbolBonguri.SelectedIndex == -1)
            {
                princSeg.IsEnabled = false;
                atrasSeg.IsEnabled = false;
            }

            if (PkbolEspecial.SelectedIndex == 0)
            {
                atrasTer.IsEnabled = false;
                princTer.IsEnabled = false;
            }
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void atrasPrim_Click(object sender, RoutedEventArgs e)
        {
            PkbolNormal.SelectedIndex--;
            if (PkbolNormal.SelectedIndex == 0)
            {
                atrasPrim.IsEnabled = false;
                princPrim.IsEnabled = false;
                finalPrim.IsEnabled = true;
                delanPrim.IsEnabled = true;
            }
            else
            {
                atrasPrim.IsEnabled = true;
                princPrim.IsEnabled = true;
                finalPrim.IsEnabled = true;
                delanPrim.IsEnabled = true;
            }
            PkbolNormal.ScrollIntoView(PkbolNormal.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delanPrim_Click(object sender, RoutedEventArgs e)
        {
            PkbolNormal.SelectedIndex++;
            if (PkbolNormal.SelectedIndex == listaNorm.Count - 1)
            {
                atrasPrim.IsEnabled = true;
                princPrim.IsEnabled = true;
                finalPrim.IsEnabled = false;
                delanPrim.IsEnabled = false;
            }
            else
            {
                atrasPrim.IsEnabled = true;
                princPrim.IsEnabled = true;
                finalPrim.IsEnabled = true;
                delanPrim.IsEnabled = true;
            }
            PkbolNormal.ScrollIntoView(PkbolNormal.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void princPrim_Click(object sender, RoutedEventArgs e)
        {
            if (PkbolNormal.SelectedIndex == 0)
            {
                atrasPrim.IsEnabled = false;
                princPrim.IsEnabled = false;
                finalPrim.IsEnabled = true;
                delanPrim.IsEnabled = true;
            }
            else
            {
                PkbolNormal.SelectedIndex = 0;
                atrasPrim.IsEnabled = false;
                princPrim.IsEnabled = false;
                finalPrim.IsEnabled = true;
                delanPrim.IsEnabled = true;
            }
            PkbolNormal.ScrollIntoView(PkbolNormal.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void finalPrim_Click(object sender, RoutedEventArgs e)
        {
            if (PkbolNormal.SelectedIndex == listaNorm.Count - 1)
            {
                atrasPrim.IsEnabled = true;
                princPrim.IsEnabled = true;
                finalPrim.IsEnabled = false;
                delanPrim.IsEnabled = false;
            }
            else
            {
                PkbolNormal.SelectedIndex = listaNorm.Count - 1;
                atrasPrim.IsEnabled = true;
                princPrim.IsEnabled = true;
                finalPrim.IsEnabled = false;
                delanPrim.IsEnabled = false;
            }
            PkbolNormal.ScrollIntoView(PkbolNormal.SelectedItem);
        }

        // BTNS BONGURIS
        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void atrasSeg_Click(object sender, RoutedEventArgs e)
        {
            PkbolBonguri.SelectedIndex--;
            if (PkbolBonguri.SelectedIndex == 0)
            {
                atrasSeg.IsEnabled = false;
                princSeg.IsEnabled = false;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;
            }
            else
            {
                atrasSeg.IsEnabled = true;
                princSeg.IsEnabled = true;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;
            }
            PkbolBonguri.ScrollIntoView(PkbolBonguri.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delanSeg_Click(object sender, RoutedEventArgs e)
        {
            PkbolBonguri.SelectedIndex++;
            if (PkbolBonguri.SelectedIndex == listaBongu.Count - 1)
            {
                atrasSeg.IsEnabled = true;
                princSeg.IsEnabled = true;
                finalSeg.IsEnabled = false;
                delanSeg.IsEnabled = false;
            }
            else
            {
                atrasSeg.IsEnabled = true;
                princSeg.IsEnabled = true;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;
            }

            if(PkbolBonguri.SelectedIndex == 0)
            {
                atrasSeg.IsEnabled = false;
                princSeg.IsEnabled = false;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;

            }
            PkbolBonguri.ScrollIntoView(PkbolBonguri.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void princSeg_Click(object sender, RoutedEventArgs e)
        {
            if (PkbolBonguri.SelectedIndex == 0)
            {
                atrasSeg.IsEnabled = false;
                princSeg.IsEnabled = false;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;
            }
            else
            {
                PkbolBonguri.SelectedIndex = 0;
                atrasSeg.IsEnabled = false;
                princSeg.IsEnabled = false;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;
            }
            PkbolBonguri.ScrollIntoView(PkbolBonguri.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void finalSeg_Click(object sender, RoutedEventArgs e)
        {
            if (PkbolBonguri.SelectedIndex == listaBongu.Count - 1)
            {
                atrasSeg.IsEnabled = true;
                princSeg.IsEnabled = true;
                finalSeg.IsEnabled = false;
                delanSeg.IsEnabled = false;
            }
            else
            {
                PkbolBonguri.SelectedIndex = listaBongu.Count - 1;
                atrasSeg.IsEnabled = true;
                princSeg.IsEnabled = true;
                finalSeg.IsEnabled = false;
                delanSeg.IsEnabled = false;
            }
            PkbolBonguri.ScrollIntoView(PkbolBonguri.SelectedItem);
        }

        // BTNS ESPECIALES
        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void atrasTer_Click(object sender, RoutedEventArgs e)
        {
            PkbolEspecial.SelectedIndex--;
            if (PkbolEspecial.SelectedIndex == 0)
            {
                atrasTer.IsEnabled = false;
                princTer.IsEnabled = false;
                finalTer.IsEnabled = true;
                delanTer.IsEnabled = true;
            }
            else
            {
                atrasTer.IsEnabled = true;
                princTer.IsEnabled = true;
                finalTer.IsEnabled = true;
                delanTer.IsEnabled = true;
            }
            PkbolEspecial.ScrollIntoView(PkbolEspecial.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delanTer_Click(object sender, RoutedEventArgs e)
        {
            PkbolEspecial.SelectedIndex++;
            if (PkbolEspecial.SelectedIndex == listaEspe.Count - 1)
            {
                atrasTer.IsEnabled = true;
                princTer.IsEnabled = true;
                finalTer.IsEnabled = false;
                delanTer.IsEnabled = false;
            }
            else
            {
                atrasTer.IsEnabled = true;
                princTer.IsEnabled = true;
                finalTer.IsEnabled = true;
                delanTer.IsEnabled = true;
            }
            PkbolEspecial.ScrollIntoView(PkbolEspecial.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void princTer_Click(object sender, RoutedEventArgs e)
        {
            if (PkbolEspecial.SelectedIndex == 0)
            {
                atrasTer.IsEnabled = false;
                princTer.IsEnabled = false;
                finalTer.IsEnabled = true;
                delanTer.IsEnabled = true;
            }
            else
            {
                PkbolEspecial.SelectedIndex = 0;
                atrasTer.IsEnabled = false;
                princTer.IsEnabled = false;
                finalTer.IsEnabled = true;
                delanTer.IsEnabled = true;
            }
            PkbolEspecial.ScrollIntoView(PkbolEspecial.SelectedItem);
        }

        /// <summary>
        /// Control botones de movimiento del listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void finalTer_Click(object sender, RoutedEventArgs e)
        {
            if (PkbolEspecial.SelectedIndex == listaEspe.Count - 1)
            {
                atrasTer.IsEnabled = true;
                princTer.IsEnabled = true;
                finalTer.IsEnabled = false;
                delanTer.IsEnabled = false;
            }
            else
            {
                PkbolEspecial.SelectedIndex = listaEspe.Count - 1;
                atrasTer.IsEnabled = true;
                princTer.IsEnabled = true;
                finalTer.IsEnabled = false;
                delanTer.IsEnabled = false;
            }
            PkbolEspecial.ScrollIntoView(PkbolEspecial.SelectedItem);
        }

        // ITEMS EVOLUCIÓN
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPiedrasEvo;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPiedrasEvo;
        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionPiedrasEvo() // Admite numGeneracion o nombre.
        {
            // Número 10 son piedras.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/10/"))
                {
                    respuestaPiedrasEvo = await response.Content.ReadAsStringAsync();
                }

                jsonPiedrasEvo = JsonDocument.Parse(respuestaPiedrasEvo);

                for (int i = 0; i < jsonPiedrasEvo.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    LbEvoPiedrasEvo.Items.Add(jsonPiedrasEvo.RootElement.GetProperty("items")[i].GetProperty("name").ToString());
                }
            } // Using
        } // PeticionPiedrasEvo

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonMegaEvo;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaMegaEvo;
        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionMegaEvo() // Admite numGeneracion o nombre.
        {
            // Número 44 mega evoluciones
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/44/"))
                {
                    respuestaMegaEvo = await response.Content.ReadAsStringAsync();
                }

                jsonMegaEvo = JsonDocument.Parse(respuestaMegaEvo);

                for (int i = 0; i < jsonMegaEvo.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    LbEvoPiedrasMega.Items.Add(jsonMegaEvo.RootElement.GetProperty("items")[i].GetProperty("name").ToString());
                }
            } // Using
        } // PeticionMegaEvo

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonItemsEvo;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaItemsEvo;
        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionItemsEvo(string consulta, string num) // Admite numGeneracion o nombre.
        {
            // Número 44 mega evoluciones
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaItemsEvo = await response.Content.ReadAsStringAsync();
                }

                jsonItemsEvo = JsonDocument.Parse(respuestaItemsEvo);

                if (num == "10")
                {
                    try
                    {
                        TBPiedrasEvo.Text = jsonItemsEvo.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6);
                        BitmapImage img = new BitmapImage(new Uri(jsonItemsEvo.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                        ImgPiedrasEvo.Source = img;
                    } catch (IndexOutOfRangeException ex)
                    {
                        TBPiedrasEvo.Text = "No hay datos disponibles por el momento.";
                        ImgPiedrasEvo.Source = null;
                    }
                    
                } else
                {
                    try
                    {
                        TBPiedrasMega.Text = jsonItemsEvo.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6);
                        BitmapImage img = new BitmapImage(new Uri(jsonItemsEvo.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                        ImgPiedrasMega.Source = img;
                    } catch (IndexOutOfRangeException ex)
                    {
                        TBPiedrasMega.Text = "No hay datos por el momento"; 
                        ImgPiedrasMega.Source = null;
                    }
                    
                }
            } // Using
        } // PeticionItemsEvo

        /// <summary>
        /// Evento para cuando esta parte del Tabitem se inicializa. Se carga en ella dos peticiones de la API.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ItemsEvolucion_Initialized(object sender, EventArgs e)
        {
            await PeticionPiedrasEvo();
            await PeticionMegaEvo();
        }

        /// <summary>
        /// Evento de doble click en el listbox de Piedras evoluciones. Se carga el objeto que se haya elegido para obtener sus datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LbEvoPiedrasEvo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await PeticionItemsEvo(jsonPiedrasEvo.RootElement.GetProperty("items")[LbEvoPiedrasEvo.SelectedIndex].GetProperty("url").ToString().Substring(26), "10");
        }

        /// <summary>
        /// Evento de doble click en el listbox de Piedras de Mega evoluciones. Se carga el objeto que se haya elegido para obtener sus datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LbEvoPiedrasMega_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await PeticionItemsEvo(jsonMegaEvo.RootElement.GetProperty("items")[LbEvoPiedrasMega.SelectedIndex].GetProperty("url").ToString().Substring(26), "44");
        }


        // LÁMINAS DE ARCEUS
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonLamArceus;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaLamArceus;
        /// <summary>
        /// Evento de la lista de las láminas que Arceus tiene disponibles.
        /// </summary>
        private List<object> lamArceusList = new List<object>();
        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionLamArceus()
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/17/"))
                {
                    respuestaLamArceus = await response.Content.ReadAsStringAsync();
                }

                jsonLamArceus = JsonDocument.Parse(respuestaLamArceus);

                for (int i = 0; i < jsonLamArceus.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    string nombrePlate = jsonLamArceus.RootElement.GetProperty("items")[i].GetProperty("name").ToString();
                    string descripcionPlate;
                    BitmapImage img;
                    await PeticionTextoImgArceus(jsonLamArceus.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));
                    try
                    {
                        descripcionPlate = jsonTextoImgArceus.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6);
                        img = new BitmapImage(new Uri(jsonTextoImgArceus.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                        lamArceusList.Add(new { Name = nombrePlate, Description = descripcionPlate, ImageUrl = img });

                    } catch (IndexOutOfRangeException ex)
                    {
                        //descripcionPlate = "No hay datos";
                        //img = null;
                        //lamArceusList.Add(new { Name = nombrePlate, Description = descripcionPlate, ImageUrl = img });
                    }
                    
                    
                }
            } // Using
        } // PeticionLamArceus

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonTextoImgArceus;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaTextoImgArceus;
        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionTextoImgArceus(string consulta)
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaTextoImgArceus = await response.Content.ReadAsStringAsync();
                }

                jsonTextoImgArceus = JsonDocument.Parse(respuestaTextoImgArceus);
            } // Using
        } // PeticionItemsEvo

        /// <summary>
        /// Evento que se cargará al carguesa la pestaña del tabitem. Se hace una petición que obtiene todas las láminas y se añade al Data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LamArceus_Initialized(object sender, EventArgs e)
        {
            await PeticionLamArceus();
            dGridArceus.ItemsSource = lamArceusList;
        }


        // VITAMINAS
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonVitaminas;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaVitaminas;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionVitaminas()
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/26/"))
                {
                    respuestaVitaminas = await response.Content.ReadAsStringAsync();
                }

                jsonVitaminas = JsonDocument.Parse(respuestaVitaminas);


                for (int i = 0; i < jsonVitaminas.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    CBVitaminas.Items.Add(jsonVitaminas.RootElement.GetProperty("items")[i].GetProperty("name").ToString());
                }
            } // Using
        } // PeticionVitaminas

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonVitaminasInfo;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaVitaminasInfo;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionVitaminasInfo(string consulta)
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaVitaminasInfo = await response.Content.ReadAsStringAsync();
                }

                jsonVitaminasInfo = JsonDocument.Parse(respuestaVitaminasInfo);
            } // Using
        } // PeticionVitaminasInfo

        /// <summary>
        /// Evento que se carga al inciar la pestaña del apartado Vitaminas del Tab Item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CBVitaminas_Initialized(object sender, EventArgs e)
        {
            await PeticionVitaminas();
        }

        /// <summary>
        /// Evento que se cargará una vez se vaya cambiado el índice del ComboBox de vitaminas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CBVitaminas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = CBVitaminas.SelectedIndex;
            if (i == -1)
            {
                VitNombre.Content = "Nombre Vitamina";
                VitCoste.Content = "Coste vitamina";
                VitEfecto.Content = "Efecto Vitamina";
                VitImagen.Content = "Imagen vitamina elegida";
                MessageBox.Show("No has seleccionado ninún elemento del desplegable.");
            }
            else
            {
                await PeticionVitaminasInfo(jsonVitaminas.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));
                VitNombre.Content = jsonVitaminasInfo.RootElement.GetProperty("name").ToString();
                VitCoste.Content = jsonVitaminasInfo.RootElement.GetProperty("cost").ToString();
                try
                {
                    VitEfecto.Content = jsonVitaminasInfo.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString();
                }
                catch (IndexOutOfRangeException ex){VitEfecto.Content = "No hay información disponible";}

                try
                {
                    VitImg.Source = new BitmapImage(new Uri(jsonVitaminasInfo.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                    VitImagen.Content = "";
                } catch (UriFormatException ex) { VitImagen.Content = "No hay imagen disponible"; VitImg.Source = null; }
                
            }
        }


        // CARTAS
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonCartas;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaCartas;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionCartas()
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/25/"))
                {
                    respuestaCartas = await response.Content.ReadAsStringAsync();
                }

                jsonCartas = JsonDocument.Parse(respuestaCartas);


                for (int i = 0; i < jsonCartas.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    CBCartas.Items.Add(jsonCartas.RootElement.GetProperty("items")[i].GetProperty("name").ToString());
                }
            } // Using
        } // PeticionCartas

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonCartasInfo;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaCartasInfo;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionCartasInfo(string consulta)
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaCartasInfo = await response.Content.ReadAsStringAsync();
                }

                jsonCartasInfo = JsonDocument.Parse(respuestaCartasInfo);
            } // Using
        } // PeticionCartasInfo

        /// <summary>
        /// Evento para cuando el Combo Box que está en el apartado de cartas se inicialice ya cargado al cargar el componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CBCartas_Initialized(object sender, EventArgs e)
        {
            await PeticionCartas();
        }

        /// <summary>
        /// Evento para cuando el ínidice del comnobox se modifica.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CBCartas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = CBCartas.SelectedIndex;
            if (i == -1)
            {
                CartaNombre.Content = "Nombre Vitamina";
                CartaCoste.Content = "Coste vitamina";
                CartaEfecto.Content = "Efecto Vitamina";
                CartaImagen.Content = "Imagen vitamina elegida";
                MessageBox.Show("No has seleccionado ninún elemento del desplegable.");
            }
            else
            {
                await PeticionCartasInfo(jsonCartas.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));
                CartaNombre.Content = jsonCartasInfo.RootElement.GetProperty("name").ToString();
                CartaCoste.Content = jsonCartasInfo.RootElement.GetProperty("cost").ToString();
                try
                {
                    CartaEfecto.Content = jsonCartasInfo.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString();
                }
                catch (IndexOutOfRangeException ex) { CartaEfecto.Content = "No hay información disponible"; }

                try
                {
                    CartaImg.Source = new BitmapImage(new Uri(jsonCartasInfo.RootElement.GetProperty("sprites").GetProperty("default").ToString()));
                    CartaImagen.Content = "";
                }
                catch (UriFormatException ex) { CartaImagen.Content = "No hay imagen disponible"; CartaImg.Source = null; }

            }
        }


        // ITEMS DE ESTADO CURACIÓN - Healing 27, PP 28, revives 29, State Cures 30
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonHealing;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaHealing;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionHealing()
        {
            // Número 27 son cura de HP.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/27/"))
                {
                    respuestaHealing = await response.Content.ReadAsStringAsync();
                }

                jsonHealing = JsonDocument.Parse(respuestaHealing);

                for (int i = 0; i < jsonHealing.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionItemsEstado(jsonHealing.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));
                    // Imagen;NomEstado;Efecto;Coste
                    LbEstadoCura.Items.Add(new { Imagen = new BitmapImage(new Uri(jsonItemsEstado.RootElement.GetProperty("sprites").GetProperty("default").ToString())), 
                        NomEstado = jsonItemsEstado.RootElement.GetProperty("name").ToString() + "\n",
                        Efecto = jsonItemsEstado.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString() + "\n",
                        Coste = "Coste: "+jsonItemsEstado.RootElement.GetProperty("cost").ToString()
                    });
                }
            } // Using
        } // PeticionHealing

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPP;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaPP;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionPP()
        {
            // Número 28 son cura de PP.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/28/"))
                {
                    respuestaPP = await response.Content.ReadAsStringAsync();
                }

                jsonPP = JsonDocument.Parse(respuestaPP);

                for (int i = 0; i < jsonPP.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionItemsEstado(jsonPP.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));
                    // Imagen;NomEstado;Efecto;Coste
                    LbEstadoPP.Items.Add(new
                    {
                        Imagen = new BitmapImage(new Uri(jsonItemsEstado.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                        NomEstado = jsonItemsEstado.RootElement.GetProperty("name").ToString() + "\n",
                        Efecto = jsonItemsEstado.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString() + "\n",
                        Coste = "Coste: " + jsonItemsEstado.RootElement.GetProperty("cost").ToString()
                    });
                }
            } // Using
        } // PeticionPP

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonRevive;
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private string respuestaRevive;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionRevive()
        {
            // Número 29 son REVIVES.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/29/"))
                {
                    respuestaRevive = await response.Content.ReadAsStringAsync();
                }

                jsonRevive = JsonDocument.Parse(respuestaRevive);

                for (int i = 0; i < jsonRevive.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionItemsEstado(jsonRevive.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));
                    if(jsonItemsEstado.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LbEstadoRevive.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonItemsEstado.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonItemsEstado.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonItemsEstado.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString() + "\n",
                            Coste = "Coste: " + jsonItemsEstado.RootElement.GetProperty("cost").ToString()
                        });
                    }
                    
                    
                }
            } // Using
        } // PeticionRevive

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonCuraEstado;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaCuraEstado;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionCuraEstado()
        {
            // Número 30 son cura de estado.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/30/"))
                {
                    respuestaCuraEstado = await response.Content.ReadAsStringAsync();
                }

                jsonCuraEstado = JsonDocument.Parse(respuestaCuraEstado);

                for (int i = 0; i < jsonCuraEstado.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionItemsEstado(jsonCuraEstado.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonItemsEstado.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LbEstadoEstado.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonItemsEstado.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonItemsEstado.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonItemsEstado.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString() + "\n",
                            Coste = "Coste: " + jsonItemsEstado.RootElement.GetProperty("cost").ToString()
                        });
                    }
                    
                }
            } // Using
        } // PeticionCuraEstado

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonItemsEstado;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaItemsEstado;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionItemsEstado(string consulta)
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaItemsEstado = await response.Content.ReadAsStringAsync();
                }

                jsonItemsEstado = JsonDocument.Parse(respuestaItemsEstado);
            } // Using
        } // PeticionItemsEstado

        /// <summary>
        /// Evento que se ejecutará cuando se inicialce el apartado del Tab Item de Los items de estado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ItemsEstado_Initialized(object sender, EventArgs e)
        {
            await PeticionHealing();
            await PeticionPP();
            await PeticionRevive();
            await PeticionCuraEstado();
        }

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasRedStats;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasRedStats;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasRedStats()
        {
            // Reducciones stats + amistad
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/2/"))
                {
                    respuestaBayasRedStats = await response.Content.ReadAsStringAsync();
                }

                jsonBayasRedStats = JsonDocument.Parse(respuestaBayasRedStats);

                for (int i = 0; i < jsonBayasRedStats.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasRedStats.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasRedStats.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6) + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasRedStats

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasMedicina;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasMedicina;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasMedicina()
        {
            // Bayas con uso medicinal.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/3/"))
                {
                    respuestaBayasMedicina = await response.Content.ReadAsStringAsync();
                }

                jsonBayasMedicina = JsonDocument.Parse(respuestaBayasMedicina);

                for (int i = 0; i < jsonBayasMedicina.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasMedicina.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasMedicinas.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6) + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasMedicina

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasCombate;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasCombate;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasCombate()
        {
            // Bayas con uso en combate.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/4/"))
                {
                    respuestaBayasCombate = await response.Content.ReadAsStringAsync();
                }

                jsonBayasCombate = JsonDocument.Parse(respuestaBayasCombate);

                for (int i = 0; i < jsonBayasCombate.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasCombate.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasCombate.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6) + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasCombate

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasApuros;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasApuros;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasApuros()
        {
            // Bayas con uso en caso de apuros en combate.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/5/"))
                {
                    respuestaBayasApuros = await response.Content.ReadAsStringAsync();
                }

                jsonBayasApuros = JsonDocument.Parse(respuestaBayasApuros);

                for (int i = 0; i < jsonBayasApuros.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasApuros.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasApuros.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6) + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasApuros

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasCuraEstado;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasCuraEstado;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasCuraEstado()
        {
            // Bayas con uso para curar los estados.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/6/"))
                {
                    respuestaBayasCuraEstado = await response.Content.ReadAsStringAsync();
                }

                jsonBayasCuraEstado = JsonDocument.Parse(respuestaBayasCuraEstado);

                for (int i = 0; i < jsonBayasCuraEstado.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasCuraEstado.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasCuraEstado.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6) + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasCuraEstado

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasProtec;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasProtec;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasProtec()
        {
            // Bayas con uso para proteger en combates.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/7/"))
                {
                    respuestaBayasProtec = await response.Content.ReadAsStringAsync();
                }

                jsonBayasProtec = JsonDocument.Parse(respuestaBayasProtec);

                for (int i = 0; i < jsonBayasProtec.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasProtec.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasProtec.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString().Substring(6) + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasProtec

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayasCocina;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayasCocina;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayasCocina()
        {
            // Bayas con uso para cocinar.
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync("item-category/8/"))
                {
                    respuestaBayasCocina = await response.Content.ReadAsStringAsync();
                }

                jsonBayasCocina = JsonDocument.Parse(respuestaBayasCocina);

                for (int i = 0; i < jsonBayasCocina.RootElement.GetProperty("items").GetArrayLength(); i++)
                {
                    await PeticionBayas(jsonBayasCocina.RootElement.GetProperty("items")[i].GetProperty("url").ToString().Substring(26));

                    if (jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString() != string.Empty)
                    {
                        // Imagen;NomEstado;Efecto;Coste
                        LBBayasCocina.Items.Add(new
                        {
                            Imagen = new BitmapImage(new Uri(jsonBayas.RootElement.GetProperty("sprites").GetProperty("default").ToString())),
                            NomEstado = jsonBayas.RootElement.GetProperty("name").ToString() + "\n",
                            Efecto = jsonBayas.RootElement.GetProperty("effect_entries")[0].GetProperty("short_effect").ToString() + "\n",
                            Coste = "Coste: " + jsonBayas.RootElement.GetProperty("cost").ToString()
                        });
                    }

                }
            } // Using
        } // PeticionBayasCocina

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonBayas;
        /// <summary>
        /// Variable para parsear el valor del JSON y mostrarlo como cadena de STRING.
        /// </summary>
        private string respuestaBayas;

        /// <summary>
        /// Método de consulta a la API para obtener una consulta pasada por parámetro.
        /// </summary>
        /// <returns></returns>
        private async Task PeticionBayas(string consulta)
        {
            var direccion = new Uri("https://pokeapi.co/api/v2/");
            using (var httpClient = new HttpClient { BaseAddress = direccion })
            {
                using (var response = await httpClient.GetAsync(consulta))
                {
                    respuestaBayas = await response.Content.ReadAsStringAsync();
                }

                jsonBayas = JsonDocument.Parse(respuestaBayas);
            } // Using
        } // PeticionBayas

        /// <summary>
        /// Evento que se lleva a cabo cuando se carga la ventana completa del tab control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TabControl_Initialized(object sender, EventArgs e)
        {
            await PeticionBayasRedStats();
            await PeticionBayasMedicina();
            await PeticionBayasCombate();
            await PeticionBayasApuros();
            await PeticionBayasCuraEstado();
            await PeticionBayasProtec();
            await PeticionBayasCocina();
        }

    }// Clase.

} // Namespace.