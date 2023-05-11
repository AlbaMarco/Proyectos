using AppDI.Recursos;
using Org.BouncyCastle.Asn1.Esf;
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

namespace AppDI.Pags.PanelAdmin.Admin
{
    /// <summary>
    /// Lógica de interacción para VerSoporteTecnico.xaml
    /// En esta ventana se podrá ver todos los tickets de soportes, se pueden filtrar por estado [Hay tres estados: ENVIADO, EN PROCESO, FINALIZADO]
    /// Además, se puede cambiar el estado según se vayan marcando.
    /// </summary>
    public partial class VerSoporteTecnico : Page
    {
        private DB miDb;

        /// <summary>
        /// Un constructor que inicializa los componentes de la ventana, además, rellenerá la tabla con todos los tickets sin filtrar y oculta varios elementos.
        /// Además, se usa un método para hacer un registro de la persona que accedió junto a la hora y dia que accedio.
        /// </summary>
        /// <param name="db"></param>
        public VerSoporteTecnico(DB db)
        {
            InitializeComponent();
            miDb = db;
            rellenarGrid();
            comboCambiarEstado.Visibility = Visibility.Hidden;
            btnCambiarEstado.Visibility = Visibility.Hidden;
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

        /// <summary>
        /// Método utilizado para rellenar la tabla con los datos de todos los tickets de soporte.
        /// </summary>
        public void rellenarGrid()
        {
            listaDataGrid.ItemsSource = miDb.selectSoporteTodo().Tables[0].DefaultView;
        }

        /// <summary>
        /// Botón que filtra según elemento que se haya elegido en un combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFiltrarEstado_Click(object sender, RoutedEventArgs e)
        {
            if (txtEstado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione en el desplegable un elemento.");
            } else
            {
                listaDataGrid.ItemsSource = miDb.selectSoporteEstado(txtEstado.SelectedValue.ToString().Substring(38)).Tables[0].DefaultView;
            }
            
        }

        /// <summary>
        /// Este método se utiliza para visualizar la fila que se haya elegido en la tabla para poder llevar a cabo el cambió de su estado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listaDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            comboCambiarEstado.Visibility = Visibility.Visible;
            btnCambiarEstado.Visibility = Visibility.Visible;

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
                lblIdTicket.Content = columnas[0];
                lblEstadoTicket.Content = columnas[2];
            }
        }

        /// <summary>
        /// Este método cambia el estado según elemento haya seleccionado en el combo box. Si no hay ningún (que esté a null) saltará una ventana avisando.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            if (comboCambiarEstado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione en el desplegable un elemento.");
            }
            else
            {
                if(miDb.cambiarEstadoSopTec(lblIdTicket.Content.ToString(), comboCambiarEstado.SelectedValue.ToString().Substring(38)) == 1)
                resCambiarEstado.Content = "Cambiado correctamente.";
            }
        }
    }
}
