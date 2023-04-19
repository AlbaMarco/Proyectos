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

namespace AppDI.Pags.PanelAdmin
{
    /// <summary>
    /// Lógica de interacción para AddNuevosUsers.xaml
    /// </summary>
    public partial class AddNuevosUsers : Page
    {
        private DB miDb;
        /// <summary>
        /// Constructor que se la pasa por parámetros un objeto de tipo base de datos.
        /// </summary>
        /// <param name="db"></param>
        public AddNuevosUsers(DB db)
        {
            InitializeComponent();
            this.miDb = db;
        }

        /// <summary>
        /// Acción del botón click, que comprobará que todos los campos estén rellenados y, en caso de lo que esté, me pondrá en mi etiqueta que el usuario ha sido creado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(tBoxNom.Text != string.Empty || tBoxPass.Text != string.Empty || tBoxNivelUser.Text != string.Empty)
            {
                if (miDb.addUsuarios(tBoxNom.Text, tBoxPass.Text, tBoxNivelUser.Text) == 1) lblResultado.Content = "Creado correctamente";
                else { lblResultado.Content = "No creado"; }
            }
            
        }

        /// <summary>
        /// Acción del botón cancelar que dejará todos los TextBox vacios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            tBoxNom.Text = "";
            tBoxPass.Text = "";
            tBoxNivelUser.Text = "";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }
    }
}
