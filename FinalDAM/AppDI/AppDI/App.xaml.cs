using AppDI.Recursos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AppDI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Lista para el apartado Pokedex Nacional.
        /// </summary>
        public List<object> listaPkxNacional = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokedex Rojo, Azul y amarillo.
        /// </summary>
        public List<object> listaPkxRoAzAm = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokedex de Oro, plata y cristal.
        /// </summary>
        public List<object> listaPkxOroPlaCri = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokedex Rubí, Zafiro y Esmeralda.
        /// </summary>
        public List<object> listaPkxRubZafEsm = new List<object>();
        /// <summary>
        /// Lista para el apartado Diamante y Perla.
        /// </summary>
        public List<object> listaPkxDiaPer = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokedex Platino.
        /// </summary>
        public List<object> listaPkxPlatino = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokedex Oro HearthGold y Plata Soulsilver.
        /// </summary>
        public List<object> listaPkxHerSoul = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokedex Blanco y negro.
        /// </summary>
        public List<object> listaPkxBlaNeg = new List<object>();
        /// <summary>
        /// Lista para el apartado Pokemon Blanco y Negro.
        /// </summary>
        public List<object> listaPkxBlaNeg2 = new List<object>();

        /// <summary>
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonPokedex;
        /// <summary>
        /// Parsear el JSON de jsonPokedex.
        /// </summary>
        private string respuestaPokedex;
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
        /// Variable de tipo JSONDOCUMENT para poder obtener el valor en JSON obtenido de la API.
        /// </summary>
        private JsonDocument jsonForm;
        /// <summary>
        /// Variable para parsear el valor del JSON jsonForm
        /// </summary>
        private string respuestaForm;
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
                try
                {
                    using (var response = await httpClient.GetAsync(consulta))
                    {
                        respuestaForm = await response.Content.ReadAsStringAsync();
                    }
                } catch (System.Net.Http.HttpRequestException ex)
                {
                    
                }
                jsonForm = JsonDocument.Parse(respuestaForm);
            }
        }

        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el primer apartado.
        /// </summary>
        public event EventHandler<int> Actualizar;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el segundo apartado.
        /// </summary>
        public event EventHandler<int> ActualizarRoAzAm;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el tercer apartado.
        /// </summary>
        public event EventHandler<int> ActualizarOroPlaCri;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el cuarto apartado.
        /// </summary>
        public event EventHandler<int> ActualizarRubZafEsm;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el quinto apartado.
        /// </summary>
        public event EventHandler<int> ActualizarDiaPer;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el sexto apartado.
        /// </summary>
        public event EventHandler<int> ActualizarPlatino;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el séptimo apartado.
        /// </summary>
        public event EventHandler<int> ActualizarHerSoul;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el octavo apartado.
        /// </summary>
        public event EventHandler<int> ActualizarBlaNeg;
        /// <summary>
        /// Variable de tipo gestora de evento para actualizar el noveno apartado.
        /// </summary>
        public event EventHandler<int> ActualizarBlaNeg2;

        /// <summary>
        /// Hace peticiones para poder rellenar el listbox de la pantalla izquierda de la Pokeddex.
        /// </summary>
        /// <param name="numGen"></param>
        private async Task rellenarPokedex(string numGen)
        {
            await PeticionPokedex(numGen);

            int num = jsonPokedex.RootElement.GetProperty("pokemon_entries").GetArrayLength();
            for (int i = 0; i < jsonPokedex.RootElement.GetProperty("pokemon_entries").GetArrayLength(); i++)
            {
                // Para que la primera letra sea mayúscula.
                string pkm = jsonPokedex.RootElement.GetProperty("pokemon_entries")[i].GetProperty("pokemon_species").GetProperty("name").ToString();
                string idPkm = jsonPokedex.RootElement.GetProperty("pokemon_entries")[i].GetProperty("pokemon_species").GetProperty("url").ToString().Substring(42); // Número donde coge la ID.
                await PeticionFotoPkm(idPkm);

                BitmapImage img;
                if (jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString() != string.Empty)
                {
                    img = new BitmapImage(new Uri(jsonForm.RootElement.GetProperty("sprites").GetProperty("front_default").ToString()));
                } else
                {
                    img = null;
                }
               
                // CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) Esto lo que hace es sacarme la primera letra en mayúscula.
                if (numGen == "1")
                {
                    listaPkxNacional.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (Actualizar != null)
                    {
                        Actualizar.Invoke(this, i);
                    }
                } else if(numGen == "2")
                {
                    listaPkxRoAzAm.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarRoAzAm != null)
                    {
                        ActualizarRoAzAm.Invoke(this, i);
                    }
                }
                else if (numGen == "3")
                {
                    listaPkxOroPlaCri.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarOroPlaCri != null)
                    {
                        ActualizarOroPlaCri.Invoke(this, i);
                    }
                }
                else if (numGen == "4")
                {
                    listaPkxRubZafEsm.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarRubZafEsm != null)
                    {
                        ActualizarRubZafEsm.Invoke(this, i);
                    }
                }
                else if (numGen == "5")
                {
                    listaPkxDiaPer.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarDiaPer != null)
                    {
                        ActualizarDiaPer.Invoke(this, i);
                    }
                }
                else if (numGen == "6")
                {
                    listaPkxPlatino.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarPlatino != null)
                    {
                        ActualizarPlatino.Invoke(this, i);
                    }
                }
                else if (numGen == "7")
                {
                    listaPkxHerSoul.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarHerSoul != null)
                    {
                        ActualizarHerSoul.Invoke(this, i);
                    }
                }
                else if (numGen == "8")
                {
                    listaPkxBlaNeg.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarBlaNeg != null)
                    {
                        ActualizarBlaNeg.Invoke(this, i);
                    }
                }
                else if (numGen == "9")
                {
                    listaPkxBlaNeg2.Add(new { Imagen = img, NomPkm = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pkm) });
                    if (ActualizarBlaNeg2 != null)
                    {
                        ActualizarBlaNeg2.Invoke(this, i);
                    }
                }
            }
        } // Final rellenar pokedex.

        /// <summary>
        /// Método de inciio de aplicación para poder ir rellenando las Pokedex en un hilo secundario, con concurrencia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await rellenarPokedex("2"); // 151 registros
            await rellenarPokedex("3"); // 251 registros
            await rellenarPokedex("4"); // 202 registros
            await rellenarPokedex("5"); // 151 registros
            await rellenarPokedex("6"); // 210 registros
            await rellenarPokedex("7"); // 256 registros
            await rellenarPokedex("8"); // 155 registros
            await rellenarPokedex("9"); // 300 registros
            await rellenarPokedex("1"); // 1010 registros
        }

    }
}
