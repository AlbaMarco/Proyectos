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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDI.Pags
{
    /// <summary>
    /// Lógica de interacción para CrearEquipos.xaml
    /// </summary>
    public partial class CrearEquipos : Page
    {
        /// <summary>
        /// Variable de tipo base de datos, que ayuda para la gestión de la base de datos.
        /// </summary>
        private DB miDB;
        /// <summary>
        /// Variable de tipo lista que ayuda a obtener los pokemons que irán en un JSON.
        /// </summary>
        private List<string> listaPkmsJson = new List<string>();
        /// <summary>
        /// Contador que cuando llegue a sieta (se inicializa a 1) para llevar la cuenta de los pkm añadidos al equipo.
        /// </summary>
        private int contAnadir;
        /// <summary>
        /// Constructor que se le pasa por parámetro un objeto de tipo datos desde que se inicia sesióon
        /// Se inicializan sus componentes y se inicializa el contador para llevar la cuenta de los PKM.
        /// </summary>
        /// <param name="db"></param>
        public CrearEquipos(DB db)
        {
            InitializeComponent();
            miDB = db;
            contAnadir = 1;
        }

        /// <summary>
        /// Evento que se ejecutará para cuando la página se cargue, es decir, al inciio de la vida de esta ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(miDB.SaberEquipos(miDB.NomUser) != "-1")
            {
                lblEquipos.Content = "Actualmente tienes [ " + miDB.SaberEquipos(miDB.NomUser) + " ] equipos creados";
                if(miDB.SaberEquipos(miDB.NomUser) == "5") 
                { 
                    MessageBox.Show("Tienes ya el máximo de equipos creados.");
                    btnCrearEquipos.Visibility = Visibility.Hidden;
                }
                btnRecargaEquipos.Visibility = Visibility.Hidden;
                gridPrincipal.Visibility = Visibility.Hidden;
            } else
            {
                lblEquipos.Content = "Hubo un error al leer la base da datos. Por favor, recargue la página";
                btnRecargaEquipos.Visibility = Visibility.Visible;
                btnCrearEquipos.Visibility = Visibility.Hidden;
                gridPrincipal.Visibility = Visibility.Hidden;
            }
            
        }

        /// <summary>
        /// Evento de click de la barra de nevagicon, para ir a la página princiapl.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Primera());
        }

        /// <summary>
        /// Evento de click para ir a la ventana de Soporte Técnico.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoporteTecnico_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SoporteTecnico(miDB));
        }

        /// <summary>
        /// Evento de click para ir a la ventana de Acceso Registrado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccReg_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Evento de acción de click al darle al boton de crear equipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearEquipos_Click(object sender, RoutedEventArgs e)
        {
            gridPrincipal.Visibility = Visibility.Visible;
            listaPkmsJson.Clear();
        }

        /// <summary>
        /// Evento de recarga, para el caso de que error algún dato de la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecargaEquipos_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Refresh();
        }

        /// <summary>
        /// Evento del botón de búsqueda para obteener el nombre escrito en el textbox y llevar a cabo la búsqueda.
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
                lbBusqueda.Items.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
            }

            txBoxNomPkm.Text = "";
        }

        /// <summary>
        /// Método asincrono para cuando se realiza doble click en el LIST BOX deonde se mostrarán las búsqueda.
        /// Este evento te facilitará los datos del Pokemon al que se le haya dado doble click en los datos de la derecha.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lbBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tbTipo.Text = "";
            tbMovimientos.Text = "";
            txtAcierto.Text = "Acierto: ";
            txtTipoDanio.Text = "Daño: ";
            txtPoder.Text = "Poder: ";
            txtPP.Text = "PP: ";
            txtTipo.Text = "Tipo: ";

            string nom = lbBusqueda.SelectedItem.ToString();
            string[] contenido = nom.Split(' ');
            string nomPkm = contenido[6].ToLower();

            await PeticionPkm(nomPkm);
            
            tbId.Text = jsonPokemon.RootElement.GetProperty("id").ToString();
            tbNombre.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(jsonPokemon.RootElement.GetProperty("name").ToString());
            tbVida.Text = jsonPokemon.RootElement.GetProperty("stats")[0].GetProperty("base_stat").ToString();

            // TIPOS
            for (int i = 0; i < jsonPokemon.RootElement.GetProperty("types").GetArrayLength(); i++)
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

            // MOVIMIENTOS
            for (int i = 0; i < 4; i++)
            {
                int max = 4;
                await PeticionMov(jsonPokemon.RootElement.GetProperty("moves")[i].GetProperty("move").GetProperty("url").ToString().Substring(26));
                if (max > 1)
                {
                    if (max - 1 == i)
                    {
                        tbMovimientos.Text += jsonPokemon.RootElement.GetProperty("moves")[i].GetProperty("move").GetProperty("name").ToString() + ".";
                        txtAcierto.Text += jsonMov.RootElement.GetProperty("accuracy").ToString() + ".";
                        if (jsonMov.RootElement.GetProperty("damage_class").GetProperty("name").ToString() == "physical") txtTipoDanio.Text += "físico.";
                        else txtTipoDanio.Text += "especial.";

                        txtPoder.Text += jsonMov.RootElement.GetProperty("power").ToString() + ".";
                        txtPP.Text += jsonMov.RootElement.GetProperty("pp").ToString() + ".";
                        txtTipo.Text += jsonMov.RootElement.GetProperty("type").GetProperty("name").ToString() + ".";
                    }
                    else
                    {
                        tbMovimientos.Text += jsonPokemon.RootElement.GetProperty("moves")[i].GetProperty("move").GetProperty("name").ToString() + ", ";
                        txtAcierto.Text += jsonMov.RootElement.GetProperty("accuracy").ToString() + ", ";
                        if (jsonMov.RootElement.GetProperty("damage_class").GetProperty("name").ToString() == "physical") txtTipoDanio.Text += "físico, ";
                        else txtTipoDanio.Text += "especial, ";

                        txtPoder.Text += jsonMov.RootElement.GetProperty("power").ToString() + ", ";
                        txtPP.Text += jsonMov.RootElement.GetProperty("pp").ToString() + ", ";
                        txtTipo.Text += jsonMov.RootElement.GetProperty("type").GetProperty("name").ToString() + ", ";
                    }

                }
                else if (max == 1)
                {
                    tbMovimientos.Text += jsonPokemon.RootElement.GetProperty("moves")[i].GetProperty("move").GetProperty("name").ToString() + ".";
                    txtAcierto.Text += jsonMov.RootElement.GetProperty("accuracy").ToString() + ".";
                    if (jsonMov.RootElement.GetProperty("damage_class").GetProperty("name").ToString() == "physical") txtTipoDanio.Text += "físico.";
                    else txtTipoDanio.Text += "especial.";

                    txtPoder.Text += jsonMov.RootElement.GetProperty("power").ToString() + ".";
                    txtPP.Text += jsonMov.RootElement.GetProperty("pp").ToString() + ".";
                    txtTipo.Text += jsonMov.RootElement.GetProperty("type").GetProperty("name").ToString() + ".";
                }
            }
        }

        /// <summary>
        /// Evento de hacer clcik en el botón de guardar para poder guardar el pokemon en cuestion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (miDB.comprobarPkmBan(tbNombre.Text) == 0)
            {
                string pkm;
                if (contAnadir == 1)
                {
                    pkm = "{   \"pkm\": [\r\n      {\r\n         \"nombre\": \"" + tbNombre.Text + "\",\r\n         \"id\": \"" + tbId.Text + "\",\r\n         \"vida\": \"" + tbVida.Text + "\"\r\n      },";
                }
                else if (contAnadir == 6)
                {
                    pkm = "   {\r\n         \"nombre\": \"" + tbNombre.Text + "\",\r\n         \"id\": \"" + tbId.Text + "\",\r\n         \"vida\": \"" + tbVida.Text + "\"\r\n      }\r\n   ]\r\n }";
                }
                else
                {
                    pkm = "   \r\n      {\r\n         \"nombre\": \"" + tbNombre.Text + "\",\r\n         \"id\": \"" + tbId.Text + "\",\r\n         \"vida\": \"" + tbVida.Text + "\"\r\n      },\r\n   ";
                }

                contAnadir++;
                listaPkmsJson.Add(pkm);
                tbElegidos.Text += "\n" + tbNombre.Text + "     " + tbId.Text;
            } else
            {
                MessageBox.Show("Ese pokemon está baneado, por favor, seleccione otro.");
                miDB.insertarNuevoEquipoBan(Convert.ToInt32(miDB.SaberEquipos(miDB.NomUser)), Convert.ToInt32(tbId.Text));
                tbTipo.Text = "";
                tbMovimientos.Text = "";
                txtAcierto.Text = "Acierto: ";
                txtTipoDanio.Text = "Daño: ";
                txtPoder.Text = "Poder: ";
                txtPP.Text = "PP: ";
                txtTipo.Text = "Tipo: ";
                tbId.Text = "";
                tbNombre.Text = "";
                tbVida.Text = "";
            }
            
        }

        /// <summary>
        /// Evento del boton de guardar equipo, será necesario haber hecho la búsqueda y el guardado de 6 pokemon, que significará que se ha completado el equipo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarEquipo_Click(object sender, RoutedEventArgs e)
        {
            if(contAnadir == 7) // Es necesario que sea 7 porque suma al final de la otra.
            {
                gridPrincipal.Visibility = Visibility.Hidden;
                if (miDB.insertarJsonNuevoEquipo(listaPkmsJson, miDB.NomUser) == 1) MessageBox.Show("Equipo creado perfectamente.");
                lblEquipos.Content = "Actualmente tienes [ " + miDB.SaberEquipos(miDB.NomUser) + " ] equipos creados";
            } else
            {
                MessageBox.Show("Se necesitan 6 pokemons en el equipo, por el momento no hay más modalidades.");
            }
        }

        /// <summary>
        /// Evento para ver los equipos que se tienen guardados en la base de datos de un usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerEquipo_Click(object sender, RoutedEventArgs e)
        {
            tbVerEquipos.Text = "Equipo/s: \n";
            if (miDB.SaberEquipos(miDB.NomUser) == "0") MessageBox.Show("No tienes equipos, primero revísalos");
            else
            {
                List<JsonDocument> resultado = miDB.verEquipos(miDB.SaberID(miDB.NomUser));
                foreach (JsonDocument valor in resultado)
                {
                    int cont = 1;
                    tbVerEquipos.Text += "[" + cont + "] ";
                    for (int i = 0; i < valor.RootElement.GetProperty("pkm").GetArrayLength(); i++)
                    {
                        tbVerEquipos.Text += valor.RootElement.GetProperty("pkm")[i].GetProperty("id").ToString() + " " +
                            "" + valor.RootElement.GetProperty("pkm")[i].GetProperty("nombre").ToString() + "\n";
                    }
                    cont++;
                }

            }
        }

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPokemon;
        /// <summary>
        /// Variable útil para parsear los valores del JSON de Pokemon.
        /// </summary>
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
        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonMov;
        /// <summary>
        /// Variable para obtener los resultados parseados del JSON DOCUMENT.
        /// </summary>
        private string respuestaMov;
        /// <summary>
        /// Petición para obtener información sobre el movimiento seleccionado.
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        private async Task PeticionMov(string consulta)
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

    } // clase
}
