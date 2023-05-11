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

namespace AppDI.Pags.PanelAdmin
{
    /// <summary>
    /// Lógica de interacción para CrearEquiposAdmin.xaml
    /// Esta ventana sirve para poder crear equipos pokemon para poder llevar a cabo los enfrentamientos contra el bot.
    /// </summary>
    public partial class CrearEquiposAdmin : Page
    {
        private DB miDB;
        /// <summary>
        /// Lista donde se añadirán 6 variables tipo texto como un json.
        /// </summary>
        private List<string> listaPkmsJson = new List<string>();
        /// <summary>
        /// Un contador que llevará la cuenta de los pokemons que se han ido eligiendo.
        /// </summary>
        private int contAnadir;
        /// <summary>
        /// Es el contructor que se le pasará un parámetro de tipo base de datos y se inicializará el contador, poniendo al administrador los equipos que hay creados de tipo BOT.
        /// Además, se usa un método para hacer un registro de la persona que accedió junto a la hora y dia que accedio.
        /// </summary>
        /// <param name="db"></param>
        public CrearEquiposAdmin(DB db)
        {
            InitializeComponent();
            miDB = db;
            contAnadir = 1;
            lblEquipos.Content = "Actualmente hay [ " + miDB.SaberEquiposAdmin() + " ] equipos BOT";
            db.RegistroLogNuevo("Crear nuevo equipo BOT", db.NomUser, db.NivelAdmin);
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
                lbBusqueda.Items.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
            }

            txBoxNomPkm.Text = "";
        }
        /// <summary>
        /// Método que rellenará los distintos campos que hay disponibles de la información del Pokémon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lbBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // FALTA COMPROBAR LOS QUE ESTÁN BANEADOS.
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
        /// Botón que guardará los datos en una posición de la lista que le toque, comprobando si es el primer elemento o el último para abrir / cerrar el JSON.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(miDB.comprobarPkmBan(tbNombre.Text) == 0)
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
        /// Botón que obtendrá la cantidad de pokemons que se han ido seleccionado y guardando en la lista para posteriormente insertar los datos a la tabla de equipos del BOT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarEquipo_Click(object sender, RoutedEventArgs e)
        {
            if (contAnadir == 7) // Es necesario que sea 7 porque suma al final de la otra.
            {
                if (miDB.insertarJsonNuevoEquipoAdmin(listaPkmsJson) == 1) MessageBox.Show("Equipo creado perfectamente.");
                lblEquipos.Content = "Actualmente hay [ " + miDB.SaberEquiposAdmin() + " ] equipos BOT";
            }
            else
            {
                MessageBox.Show("Se necesitan 6 pokemons en el equipo, por el momento no hay más modalidades.");
            }
        }

        /// <summary>
        /// Botón que servirá para ver los equipos que han sido creados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerEquipo_Click(object sender, RoutedEventArgs e)
        {
            tbVerEquipos.Text = "Equipo/s: \n";
            if (miDB.SaberEquiposAdmin() == "0") MessageBox.Show("No hay equipos BOT.");
            else
            {
                List<JsonDocument> resultado = miDB.verEquiposAdmin();
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

        private JsonDocument jsonMov;
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

    } // Clase
}
