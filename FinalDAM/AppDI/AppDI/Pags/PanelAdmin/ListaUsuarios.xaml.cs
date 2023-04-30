using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using AppDI.Recursos;

namespace AppDI.Pags.PanelAdmin
{
    /// <summary>
    /// Lógica de interacción para ListaUsuarios.xaml
    /// </summary>
    public partial class ListaUsuarios : Page
    {
        private DB miDb;

        /// <summary>
        /// Constructor que se la pasa por parámetros un objeto de tipo base de datos.
        /// Además, rellenará mi grid con todos los datos de los usuarios excepto la contraseña.
        /// </summary>
        /// <param name="db"></param>
        public ListaUsuarios(DB db)
        {
            InitializeComponent();
            miDb = db;
            rellenarGrid();
            db.RegistroLogNuevo("Listando los usuarios", db.NomUser, db.NivelAdmin);
        }

        /// <summary>
        /// Rellenar el grid utilizado en esta ventana de Lista de Usuarios
        /// </summary>
        public void rellenarGrid()
        {
            listaDataGrid.ItemsSource = miDb.selectTodo().Tables[0].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }
    }
}
