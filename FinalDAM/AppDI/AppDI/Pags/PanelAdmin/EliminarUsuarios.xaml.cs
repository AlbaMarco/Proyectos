using AppDI.Recursos;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para EliminarUsuarios.xaml
    /// </summary>
    public partial class EliminarUsuarios : Page
    {
        private DB miDb;

        /// <summary>
        /// Constructor que se la pasa por parámetros un objeto de tipo base de datos.
        /// </summary>
        /// <param name="db"></param>
        public EliminarUsuarios(DB db)
        {
            InitializeComponent();
            this.miDb = db;
            rellenarGrid();
        }

        /// <summary>
        /// Método que coge un DS de mi clase de Base de Datos y me rellena el DataGrid
        /// </summary>
        public void rellenarGrid()
        {
            listaDataGrid.ItemsSource = miDb.selectTodo().Tables[0].DefaultView;
        }

        /// <summary>
        /// Acción del botón principal del ratón para seleccionar los datos que están en esa columna y fila, mostrándolo en el label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listaDataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<string> columnas = new List<string>();
            // Recorrer las filas seleccionadas del DataGrid
            foreach (var selectedItem in listaDataGrid.SelectedItems)
            {
                // Obtener la fila seleccionada
                DataRowView rv = (DataRowView)selectedItem;
                
                foreach (var columna in listaDataGrid.Columns)
                {
                    // Valor de la columna
                    columnas.Add(rv.Row[columna.Header.ToString()].ToString());
                }
                lbUser.Content = columnas[0];
                lbAdmin.Content = columnas[1];
                lbNivel.Content = columnas[2];
                
            }
            
        }

        /// <summary>
        /// Acción del botón de eliminar para la eliminación del usuario en la BD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(miDb.eliminarUsuarios((string)lbUser.Content, (string)lbAdmin.Content, (string)lbNivel.Content) == 1) { lbCorrecto.Content = "Correcto."; }
            else { lbCorrecto.Content = "Error."; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }
    }
}
