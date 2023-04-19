using AppDI.Pags;
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

namespace AppDI.Recursos
{
    /// <summary>
    /// Lógica de interacción para ExpanderAdmin.xaml
    /// </summary>
    public partial class ExpanderAdmin : UserControl
    {
        /// <summary>
        /// Propiedad para obtener el núm de boton pulsado.
        /// </summary>
        public int NumBtn { get; set; }

        /// <summary>
        /// Constructor, no se le pasa nada por parámetro.
        /// </summary>
        public ExpanderAdmin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Si es la segunda opción, mi propiedad la dejará en dos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModNivel_Click(object sender, RoutedEventArgs e)
        {
            NumBtn = 2;
        }

        /// <summary>
        /// Si es la primera opción, mi propiedad la dejará en uno.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListaUsers_Click(object sender, RoutedEventArgs e)
        {
            NumBtn = 1;
        }

        /// <summary>
        /// Si es la tercera opción, mi propiedad la dejará en tres.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            NumBtn = 3;
        }

        /// <summary>
        /// Si es la cuarta opción, mi propiedad la dejará en cuatro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliUser_Click(object sender, RoutedEventArgs e)
        {
            NumBtn = 4;
        }

        /// <summary>
        /// Si es la quinta opción, mi propiedad la dejará en cinco.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModAdmin_Click(object sender, RoutedEventArgs e)
        {
            NumBtn = 5;
        }
    }
}
