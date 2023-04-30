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

namespace AppDI.Pags
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : Page
    {
        private DB miDb;
        /// <summary>
        /// Constructor donde se crea un objeto de tipo Base de Datos.
        /// </summary>
        public Registro()
        {
            InitializeComponent();
            miDb = new DB();
        }

        /// <summary>
        /// Accion del click donde se comprueban varias cosas:
        /// Primero, que los textos no estén vacios.
        /// Si es correcto, comprueba que ambas contraseñas coincidan.
        /// Por último, si coinciden las contraseñas, se hace un insert a la tabla de base de datos.
        /// Por defecto será nivel uno y no será administración.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            if(userAcc.Text != string.Empty && passAcc.Password != string.Empty && passAccConfirma.Password != string.Empty)
            {
                if(passAcc.Password.ToString() == passAccConfirma.Password.ToString())
                {
                    if(miDb.registroUsuarios(userAcc.Text, passAcc.Password.ToString()) == 1)
                    {
                        MessageBox.Show("Creado correctamente, pruebe a iniciar sesión.");
                    } else if (miDb.registroUsuarios(userAcc.Text, passAcc.Password.ToString()) == 0)
                    {
                        MessageBox.Show("Hubo un error en el registro.");
                    }
                } else
                {
                    MessageBox.Show("Las contraseñas no coinciden.");
                }
            } else
            {
                MessageBox.Show("Rellene todos los campos, por favor.");
            }
        }*/
    }
}
