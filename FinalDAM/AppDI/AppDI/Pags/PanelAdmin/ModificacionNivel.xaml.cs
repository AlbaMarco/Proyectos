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
    /// Lógica de interacción para ModificacionNivel.xaml
    /// </summary>
    public partial class ModificacionNivel : Page
    {
        private DB miDb;
        /// <summary>
        /// Constructor que se la pasa por parámetros un objeto de tipo base de datos.
        /// Rellenará el listbox y pondrá en visible los TextBox.
        /// </summary>
        /// <param name="db"></param>
        public ModificacionNivel(DB db)
        {
            InitializeComponent();
            miDb = db;
            lbNom.ItemsSource = miDb.selectNombres();
            tBoxNom.Visibility = Visibility.Hidden;
            tBoxNivelUser.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Acción de un doble click al listbox, donde se pondrá el nombre y el nivel de ese usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbNom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            lblNom.Content = lbNom.SelectedItem.ToString();
            lblNivelUser.Content = miDb.nivelUsuario(lbNom.SelectedItem.ToString());
        }

        /// <summary>
        /// Acción asignada al label del nombre, donde pondrá en invisible el textBox para modificar el nivel y visible el textBox para modificar el nombre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblNom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tBoxNom.Visibility = Visibility.Visible;
            tBoxNivelUser.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Acción asignada al label del nombre, donde pondrá en invisible el textBox para modificar el nombre y visible el textBox para modificar el nivel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblNivelUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tBoxNom.Visibility = Visibility.Hidden;
            tBoxNivelUser.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Acción del botón de cancelar que ocultará los dos textBox y pondrá por defecto el contenido de las etiquetas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            tBoxNom.Visibility = Visibility.Hidden;
            tBoxNivelUser.Visibility = Visibility.Hidden;
            lblNom.Content = "Nombre";
            lblNivelUser.Content = "Nivel Usuario";
        }

        /// <summary>
        /// Acción del botón de guardar donde se comprobará cuál etiqueta está visible y, dependiendo de ella, se hará un actualizar nombre o actualizar nivel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(tBoxNom.Visibility == Visibility.Visible)
            {
                // Cambiará el nombre en el Update.
                int num = miDb.actualizarNombre(lblNom.Content.ToString(), tBoxNom.Text);
                lblResultados.Content = "Filas cambiadas correctamente: " + num;
            }
            else if (tBoxNivelUser.Visibility == Visibility.Visible)
            {
                // Cambiará el nivel de usuario en el Update.
                int num = miDb.actualizarNivel(lblNom.Content.ToString(), tBoxNivelUser.Text);
                lblResultados.Content = "Filas cambiadas correctamente: " + num;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }
    }
}
