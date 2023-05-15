using AppDI.Recursos;
using MySql.Data.MySqlClient;
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
    /// Lógica de interacción para SoporteTecnico.xaml
    /// </summary>
    public partial class SoporteTecnico : Page
    {
        /// <summary>
        /// Variable de tipo base de datos, recurso creado en la carpeta de "Recursos", donde se leeran todos los datos obtendos de la parte vista.
        /// </summary>
        private DB miDB;
        /// <summary>
        /// Constructor que inicializa los componentes de la ventana.
        /// </summary>
        public SoporteTecnico(DB db)
        {
            InitializeComponent();
            miDB = db;
        }

        /// <summary>
        /// Hacer click en la barra le llevará a la página de atrás.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Botón que hace click una vez el contenido está escrito. Se comprueba que no esté vacio para que no inserte un registro vacio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if (txtContenido.Text != string.Empty)
            {
                if (miDB.EnviarSoporte(txtContenido.Text) == 1) MessageBox.Show("Se ha enviado correctamente al equipo de soporte técico. Gracias por su granito de arena."); 
                else MessageBox.Show("Hubo un error a la hora de enviar el ticket.");
            }
            else MessageBox.Show("Rellene el cuadro de texto. No se puede enviar vacio.");
           
        }
    }
}
