using MySql.Data.MySqlClient;
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

namespace AppDI.Pags
{
    /// <summary>
    /// Lógica de interacción para SoporteTecnico.xaml
    /// </summary>
    public partial class SoporteTecnico : Page
    {
        private MySqlConnection conexion;
        private MySqlCommand comando;
        public SoporteTecnico()
        {
            InitializeComponent();
        }
        // INSERT INTO `SOPORTETECNICO` (`ID_TICKET`, `TXT_TICKET`, `ESTADO`, `FECHA_ENTRADA`) VALUES (NULL, '', '', CURRENT_TIMESTAMP)
        private void Menu_Inicio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Primera());
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            conexion = new MySqlConnection("server=db4free.net;uid=albaroot;pwd=albaroot;database=appfinal");

            comando = new MySqlCommand("INSERT INTO `SOPORTETECNICO` (`ID_TICKET`, `TXT_TICKET`, `ESTADO`, `FECHA_ENTRADA`) VALUES (NULL, '"+ txtContenido.Text + "', 'Enviado', CURRENT_TIMESTAMP)", conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            if(num == 1) { MessageBox.Show("Se ha enviado correctamente al equipo de soporte técico. Gracias por su granito de arena."); }
            conexion.Close();
        }
    }
}
