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
    /// En esta ventana se podrá añadir nuevos usuarios. Su funcionamiento es facil, se pone un nombre, un nivel de usuario y una contraseña la cual será hasheada.
    /// </summary>
    public partial class AddNuevosUsers : Page
    {
        private DB miDb;
        /// <summary>
        /// Constructor que se la pasa por parámetros un objeto de tipo base de datos.
        /// Además, se usa un método para hacer un registro de la persona que accedió junto a la hora y dia que accedio.
        /// </summary>
        /// <param name="db"></param>
        public AddNuevosUsers(DB db)
        {
            InitializeComponent();
            this.miDb = db;
            db.RegistroLogNuevo("Añadir nuevo usuario", db.NomUser, db.NivelAdmin);
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
                else if (miDb.addUsuarios(tBoxNom.Text, tBoxPass.Text, tBoxNivelUser.Text) != -1) lblResultado.Content = "Existe ya el usuario."; 
                else { lblResultado.Content = "No creado. Hubo un error."; }
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

        /// <summary>
        /// Evento al cargar la página que evita que la navegación cargue ya que para el proceso.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }
    }
}
