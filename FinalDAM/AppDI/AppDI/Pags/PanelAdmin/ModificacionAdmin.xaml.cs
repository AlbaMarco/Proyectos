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
    /// Lógica de interacción para ModificacionAdmin.xaml
    /// </summary>
    public partial class ModificacionAdmin : Page
    {
        private DB miDb;
        /// <summary>
        /// Constructor, recibe por parámetro un objeto de Base de datos.
        /// Además, deshabilitará los botones.
        /// </summary>
        /// <param name="db"></param>
        public ModificacionAdmin(DB db)
        {
            InitializeComponent();
            miDb = db;
            lbNom.ItemsSource = miDb.selectNombres();
            rBtnSi.IsEnabled = false;
            rBtnNo.IsEnabled = false;
            btnConfirmar.IsEnabled = false;
        }
        /// <summary>
        /// Acción de doble click de mi ListBox, se activará el botón de confirmación y, dependiendo de nivel de usuario, activará unos radio botones u otros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbNom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            rBtnSi.IsChecked = false;
            rBtnNo.IsChecked = false;
            btnConfirmar.IsEnabled = true;
            
            if(miDb.selectAdmin(lbNom.SelectedItem.ToString()) == 1)
            {
                rBtnSi.IsEnabled = false;
                rBtnNo.IsEnabled = true;
            } else
            {
                rBtnSi.IsEnabled = true;
                rBtnNo.IsEnabled = false;
            }
        }

        /// <summary>
        /// Acción de click del botón de confirmación.
        /// Si se hizo con éxito, se mostrará en mi label sin contenido que tuvo éxito, sino, que hubo algún error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if(rBtnSi.IsChecked == true)
            {
                if(miDb.actualizarAdmin(lbNom.SelectedItem.ToString(), "1") == 1)
                {
                    lbResultado.Content = "Cambiado con éxito";
                } else
                {
                    lbResultado.Content = "Hubo algún tipo de error";
                }
                
            } else if (rBtnNo.IsChecked == true)
            {
                if (miDb.actualizarAdmin(lbNom.SelectedItem.ToString(), "0") == 1)
                {
                    lbResultado.Content = "Cambiado con éxito";
                }
                else
                {
                    lbResultado.Content = "Hubo algún tipo de error";
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }
    }
}
