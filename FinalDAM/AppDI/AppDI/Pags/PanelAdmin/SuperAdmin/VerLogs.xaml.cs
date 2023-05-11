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

namespace AppDI.Pags.PanelAdmin.SuperAdmin
{
    /// <summary>
    /// Lógica de interacción para VerLogs.xaml
    /// Ventana con acceso super administrador. En ella se podrá ver todos los logs que hayan en la tabla. Nadie puede acceder a ellos y el super administrador sólo lo podrá visualizar.
    /// </summary>
    public partial class VerLogs : Page
    {
        private DB miDb;
        /// <summary>
        /// Constructor que rellena el data grid con los datos de toda la tabla de LOGS.
        /// </summary>
        /// <param name="db"></param>
        public VerLogs(DB db)
        {
            InitializeComponent();
            miDb = db;
            rellenarGrid();
        }

        /// <summary>
        /// Rellenar el grid utilizado en esta ventana de Lista de Usuarios
        /// </summary>
        public void rellenarGrid()
        {   
            listaDataGrid.ItemsSource = miDb.selectLogs().Tables[0].DefaultView;
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
    } // Clase
}
