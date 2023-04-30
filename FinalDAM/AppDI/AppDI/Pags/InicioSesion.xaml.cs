using AppDI.Recursos;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDI.Pags
{
    /// <summary>
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class InicioSesion : Page
    {
        private DB miBD;
        /// <summary>
        /// Una ventana custom.
        /// </summary>
        private Window miVentana { get; set; }
        /// <summary>
        /// Propiedad que controla las veces que ha intentado iniciar sesión.
        /// </summary>
        private int Contador { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public InicioSesion()
        {
            InitializeComponent();
            miBD = new DB();
            Contador = 0;
        }
        /// <summary>
        /// Al darle click al botón de inciio de sesión, se conectará o saldrá un error. Después de tres errores, lo llevará a la página de acceso gratuito.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void inSesion_Click(object sender, RoutedEventArgs e)
        {
            if (miBD.ConectarBD(userAcc.Text, passAcc.Password.ToString()))
            { 
                if (miBD.EsAdmin() || miBD.EsSuperAdmin())
                {
                    eleccionAdmin();
                } else
                {
                    this.NavigationService.Navigate(new ISCorrecto(miBD));
                }
            }
            else
            {
                // En caso de que falle tres veces, se lo llevará a una ventana de acceso gratuito.
                if(Contador == 2)
                {
                    MessageBox.Show("Has fallado demasiado veces al iniciar sesión, redirigiendote.");
                    this.NavigationService.GoBack();
                } else
                {
                    MessageBox.Show("Credenciales incorrectos o faltó rellenar algún campo.");
                    Contador++;
                }
            }
        }

        /// <summary>
        /// Desplegable en cuál se mostrará dos botones de SI o NO controlando un evento que te llevará a una página u a otro dependiendo de cual se pulse.
        /// </summary>
        private void eleccionAdmin ()
        {
            Window custom = new Window();
            custom.Title = "Acceso Administrador";
            custom.Width = 400; // 300
            custom.Height = 100;
            Color c = Color.FromRgb(55, 97, 168); // #3761a8
            SolidColorBrush b = new SolidColorBrush(c);
            custom.Background = b;
            custom.Icon = new BitmapImage(new Uri("../../../Resources/Pokeball.png", UriKind.Relative));

            FontFamily fuente = new FontFamily(new Uri("pack://application:,,,/"), "./Fuentes/#Pocket Monk");

            Thickness margin = new Thickness(5, 0, 10, 0);
            TextBlock text1 = new TextBlock();
            text1.Text = "¿Desea ir al panel de control de administrador?";
            text1.VerticalAlignment = VerticalAlignment.Center;
            text1.HorizontalAlignment = HorizontalAlignment.Center;
            text1.FontSize = 17;
            text1.Margin = margin;
            text1.FontFamily = fuente;
            text1.Foreground = Brushes.Gold;

            Grid grid = new Grid();
            // Columnas
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(200, GridUnitType.Pixel); // Estaba a 150 ya que el ancho era 300.
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(200, GridUnitType.Pixel); // Igual.
            grid.ColumnDefinitions.Add(col1); 
            grid.ColumnDefinitions.Add(col2);

            // Filas
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(20, GridUnitType.Pixel);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(50, GridUnitType.Pixel);
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            grid.Children.Add(text1);

            // Uso la propiedad SetRow del objeto Grid.
            Grid.SetRow(text1, 0);
            Grid.SetColumn(text1, 0);
            Grid.SetColumnSpan(text1, 2);
            
            // Creo dos botones, uno para acceder a la ventana admin si se le da click y otro que no.
            Button btnSi = new Button();
            btnSi.Content = "Sí";
            btnSi.FontFamily = fuente;
            btnSi.Height = 30;
            btnSi.FontSize = 18;
            btnSi.Background = b;
            btnSi.Foreground = Brushes.Gold;
            btnSi.BorderBrush = Brushes.Gold;
            grid.Children.Add(btnSi);
            Grid.SetRow(btnSi, 1);
            Grid.SetColumn(btnSi, 0);

            Button btnNo = new Button();
            btnNo.Content = "No";
            btnNo.FontFamily = fuente;
            btnNo.Height = 30;
            btnNo.FontSize = 18;
            btnNo.Background = b;
            btnNo.Foreground = Brushes.Gold;
            btnNo.BorderBrush = Brushes.Gold;
            grid.Children.Add(btnNo);
            Grid.SetRow(btnNo, 1);
            Grid.SetColumn(btnNo, 1);

            miVentana = custom;
            // Hacemos los eventos con el EventHandler.
            btnSi.Click += new RoutedEventHandler(clickSi);
            btnNo.Click += new RoutedEventHandler(clickNo);

            custom.Content = grid;
            custom.ShowDialog();
        } // Para la ventana emergente

        /// <summary>
        /// Evento propio del click al botón SI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickSi (object sender, EventArgs e)
        {
            miVentana.Close();
            this.NavigationService.Navigate(new AdminControl(miBD));
        }

        /// <summary>
        /// Evento propio del click al botón NO.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickNo(object sender, EventArgs e)
        {
            miVentana.Close();
            this.NavigationService.Navigate(new ISCorrecto(miBD));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.StopLoading();
        }

        private void menuTxt_Click(object sender, RoutedEventArgs e)
        {
            userAcc.Text = "";
        }

        private void menuPassword_Click(object sender, RoutedEventArgs e)
        {
            passAcc.Password = "";
        }
    }
}
