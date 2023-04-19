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

namespace AppDI.Recursos
{
    /// <summary>
    /// Lógica de interacción para TabItemTier1.xaml
    /// </summary>
    public partial class TabItemTier1 : UserControl
    {
        private JsonDocument jsonPokedex;
        private string respuestaPokedex;

        private JsonDocument jsonPkm;
        private string respuestaPkm;

        private JsonDocument jsonForm;
        private string respuestaForm;

        private JsonDocument jsonEvolu;
        private string respuestaEvolu;

        public string idPkm { get; set; }
        public string nombrePkm { get; set; }
        public TabItemTier1()
        {
            InitializeComponent();
            evoluciones.IsEnabled = false;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //await PeticionMovsGen(Convert.ToString(selecGen.SelectedIndex + 1));
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

        private JsonDocument jsonCadEvo;
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
            evoluciones.IsEnabled = false;
            BusquedaAPI.Items.Clear();

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
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (nacional.IsChecked == true)
            {
                rellenarPokedex("1");
            }
            else if (roAzAm.IsChecked == true)
            {
                rellenarPokedex("2");
            }
            else if (oroPlaCris.IsChecked == true)
            {
                rellenarPokedex("3");
            }
            else if (rubSafEm.IsChecked == true)
            {
                rellenarPokedex("4");
            }
            else if (diaPer.IsChecked == true)
            {
                rellenarPokedex("5");
            }
            else if (platino.IsChecked == true)
            {
                rellenarPokedex("6");
            }
            else if (heartSoul.IsChecked == true)
            {
                rellenarPokedex("7");
            }
            else if (blanNeg.IsChecked == true)
            {
                rellenarPokedex("8");
            }
            else if (blanNeg2.IsChecked == true)
            {
                rellenarPokedex("9");
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

                // Para sacar su ID y poddedr hacer la consulta a su cadena evolutiva, primero haremos la búsqueda pkm y se mostrará su información a la parte izq.
                await PeticionPokemon(nomPkm);

                idPkm = jsonPkm.RootElement.GetProperty("id").ToString();
                nombrePkm = jsonPkm.RootElement.GetProperty("name").ToString();

                PkmSelecc.Items.Add(new { TituloPkm = "Altura:", NomPkm = jsonPkm.RootElement.GetProperty("height").ToString() + "cm" });
                PkmSelecc.Items.Add(new { TituloPkm = "Peso:", NomPkm = jsonPkm.RootElement.GetProperty("weight").ToString() + "kg" });
                if (jsonPkm.RootElement.GetProperty("is_default").ToString() == "True")
                {
                    PkmSelecc.Items.Add(new { TituloPkm = "¿Inicial?", NomPkm = "Sí" });
                }
                else
                {
                    PkmSelecc.Items.Add(new { TituloPkm = "¿Inicial?", NomPkm = "No" });
                }

                PkmSelecc.Items.Add(new { TituloPkm = "Tipo/s:", NomPkm = jsonPkm.RootElement.GetProperty("types").GetArrayLength().ToString() });
                for (int i = 0; i < jsonPkm.RootElement.GetProperty("types").GetArrayLength(); i++)
                {
                    PkmSelecc.Items.Add(new { TituloPkm = "   " + jsonPkm.RootElement.GetProperty("types")[i].GetProperty("type").GetProperty("name").ToString(), NomPkm = " " });
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

        private JsonDocument jsonMovsGen;
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

                for (int i = 0; i < jsonMovsGen.RootElement.GetProperty("moves").GetArrayLength(); i++)
                {
                    lbMovs.Items.Add(jsonMovsGen.RootElement.GetProperty("moves")[i].GetProperty("name").ToString());
                }
            }
        }

        private JsonDocument jsonMov;
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
        }

        // FIN MOVIMIENTOS



        // TIPOS POKEMON

        private JsonDocument jsonTipGen;
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

                

            }
        } // Peticion de la generación tipos.

        private JsonDocument jsonTip;
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
        } // LbTipos

        // FIN ETIQUETA TIPOS.


        // INICIO POKEBOLAS
        private JsonDocument jsonPkballCat;
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
                if (lbMovs != null) { lbMovs.Items.Clear(); }
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


        private JsonDocument jsonPball;
        private string respuestaPball;
        private List<object> listaNorm = new List<object>();
        private List<object> listaEspe = new List<object>();
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

            PkbolBonguri.SelectedIndex = 0;

            if (PkbolNormal.SelectedIndex == 0)
            {
                atrasPrim.IsEnabled = false;
                princPrim.IsEnabled = false;
            }

            if (PkbolBonguri.SelectedIndex == 0)
            {
                atrasSeg.IsEnabled = false;
                princSeg.IsEnabled = false;
                
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
            if (PkbolNormal.SelectedIndex == 0)
            {
                atrasPrim.IsEnabled = false;
                princPrim.IsEnabled = false;
                finalPrim.IsEnabled = true;
                delanPrim.IsEnabled = true;
            }
            else
            {
                PkbolNormal.SelectedIndex--;
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
            if (PkbolNormal.SelectedIndex == listaNorm.Count - 1)
            {
                atrasPrim.IsEnabled = true;
                princPrim.IsEnabled = true;
                finalPrim.IsEnabled = false;
                delanPrim.IsEnabled = false;
            }
            else
            {
                PkbolNormal.SelectedIndex++;
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
            if (PkbolBonguri.SelectedIndex == -1)
            {
                atrasSeg.IsEnabled = false;
                princSeg.IsEnabled = false;
                finalSeg.IsEnabled = true;
                delanSeg.IsEnabled = true;
            }
            else
            {
                PkbolBonguri.SelectedIndex--;
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
            if (PkbolBonguri.SelectedIndex == listaBongu.Count - 1)
            {
                atrasSeg.IsEnabled = true;
                princSeg.IsEnabled = true;
                finalSeg.IsEnabled = false;
                delanSeg.IsEnabled = false;
            }
            else
            {
                PkbolBonguri.SelectedIndex++;
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
            if (PkbolEspecial.SelectedIndex == 0)
            {
                atrasTer.IsEnabled = false;
                princTer.IsEnabled = false;
                finalTer.IsEnabled = true;
                delanTer.IsEnabled = true;
            }
            else
            {
                PkbolEspecial.SelectedIndex--;
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
            if (PkbolEspecial.SelectedIndex == listaEspe.Count - 1)
            {
                atrasTer.IsEnabled = true;
                princTer.IsEnabled = true;
                finalTer.IsEnabled = false;
                delanTer.IsEnabled = false;
            }
            else
            {
                PkbolEspecial.SelectedIndex++;
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
    }// Clase.

} // Namespace.