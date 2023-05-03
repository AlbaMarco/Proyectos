using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Policy;

namespace AppDI.Recursos
{
    /// <summary>
    /// Clase de control de mi base de datos.
    /// </summary>
    public class DB // Acceso a la Base de datos.
    {
        private MySqlConnection conexion;
        private MySqlCommand comando;
        private MySqlDataReader readSQL;
        private MySqlDataAdapter adaptador;

        private MySqlDataReader readUser;
        private DataSet ds;
        /// <summary>
        /// Propiedadad para obtener la conexión.
        /// </summary>
        private string Conex { get; set; }

        private bool esAdmin;
        private bool esSuperAdmin;
        /// <summary>
        /// Propiedad para saber el nivel del usuario conectado. De 1 a 4.
        /// </summary>
        public string nivelUserConectado { get; set; }
        /// <summary>
        /// Propiedad para obtener el nombre de usuario.
        /// </summary>
        public string NomUser { get; set; }
        /// <summary>
        /// Propiedad para saber el nivel del administrador. 1 Normal, 2 SuperAdmin
        /// </summary>
        public int NivelAdmin { get; set; }

        /// <summary>
        /// Método de conexción con la base de datos embebida. Además, se almacenará el nombre y si es administrador en propiedades.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool ConectarBD(string user, string pass)
        {
            Conex = "server=db4free.net;uid=albaroot;pwd=albaroot;database=appfinal";
            if (conexion != null) { conexion.Close(); }

            try
            {
                conexion = new MySqlConnection("server=db4free.net;uid=albaroot;pwd=albaroot;database=appfinal");
                conexion.Open();

                comando = new MySqlCommand("SELECT PASS FROM USERS WHERE USER = @user", conexion);
                comando.Parameters.AddWithValue("@user", user);
                readUser = comando.ExecuteReader();


                if (readUser.Read())
                {
                    if (BCrypt.Net.BCrypt.Verify(pass, readUser["PASS"].ToString()))
                    {
                        // Las contraseñas coinciden, el usuario ha iniciado sesión exitosamente
                        readUser.Close();

                        // Se actualiza el campo de la fecha para saber que día ha entrado.
                        DateTime fechaActual = DateTime.Now;
                        string fecha = fechaActual.ToString("yyyy-MM-dd"); // MM para que sean meses.
                        comando = new MySqlCommand("UPDATE USERS SET ULT_VISITA = '"+fecha+"' where USER = @usuarios ", conexion);
                        comando.Parameters.AddWithValue("@usuarios", user);
                        comando.ExecuteNonQuery();

                        // Selecciona los datos de la persona que ha entrado en la aplicación.
                        comando = new MySqlCommand("SELECT * FROM USERS WHERE USER = @usuario", conexion);
                        comando.Parameters.AddWithValue("@usuario", user);

                        readSQL = comando.ExecuteReader();

                        if (readSQL.Read())
                        {
                            if (readSQL["LEVELA"].ToString() == "1")
                            {
                                esAdmin = true;
                                NivelAdmin = 1;
                            }
                            else if (readSQL["LEVELA"].ToString() == "2")
                            {
                                esSuperAdmin = true;
                                NivelAdmin = 2;
                            }
                            else
                            {
                                esAdmin = false;
                                esSuperAdmin = false;
                                NivelAdmin = 0;
                            }

                            nivelUserConectado = readSQL["LEVELU"].ToString();
                            NomUser = readSQL["USER"].ToString();
                            readSQL.Close();
                            conexion.Close();
                            return true;
                        }

                        readUser.Close();
                        conexion.Close();

                        return false;
                    }
                    else
                    {
                        // Las contraseñas no coinciden, mostrar un mensaje de error
                        MessageBox.Show("NO COINCIDEN PASS");
                        readUser.Close();
                        return false;
                    }
                } 
                else
                {
                    return false;
                }

            }
            catch (MySqlException ex)
            {
                ex.GetBaseException();
                return false;
            } finally { conexion.Close(); };
        } // ConectarDB

        /// <summary>
        /// Método que dice si el usuario es adminsitrador o no.
        /// </summary>
        /// <returns></returns>
        public bool EsAdmin()
        {
            return esAdmin;
        }

        /// <summary>
        /// Método que dice si el usuario es super adminitrador o no.
        /// </summary>
        /// <returns></returns>

        public bool EsSuperAdmin()
        {
            return esSuperAdmin;
        }


        /// <summary>
        /// Select de la tabla Usuarios de todo el contenido, excepto de la contraseña.
        /// </summary>
        /// <returns></returns>
        public DataSet selectTodo()
        {
            conexion = new MySqlConnection(Conex);
            comando = new MySqlCommand("SELECT USER, ULT_VISITA, LEVELU, LEVELA from USERS", conexion);

            conexion.Open();
            adaptador = new MySqlDataAdapter(comando);
            ds = new DataSet();

            adaptador.Fill(ds, "users");
            conexion.Close();

            return ds;
        }

        /// <summary>
        /// Sólo me saca los nombres de la tabla usuarios.
        /// </summary>
        /// <returns></returns>
        public List<string> selectNombres()
        {
            conexion = new MySqlConnection(Conex);
            comando = new MySqlCommand("SELECT USER from USERS", conexion);

            conexion.Open();
            adaptador = new MySqlDataAdapter(comando);
            ds = new DataSet();

            adaptador.Fill(ds, "users");

            List<string> nombres = new List<string>();
            foreach(DataRow dr in ds.Tables["users"].Rows)
            {
                nombres.Add(dr["USER"].ToString());
            }

            conexion.Close();

            return nombres;
        }

        /// <summary>
        /// Método para saber si es administrador bajo la persona. 
        /// Devolverá 1 si es que es, 0 si no lo es y -1 si da error la consulta.
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public int selectAdmin(string nom)
        {
            try
            {
                comando = new MySqlCommand("Select LEVELA from USERS where USER ='" + nom + "';", conexion);

                conexion.Open();
                readSQL = comando.ExecuteReader();

                if (readSQL.Read())
                {
                    if (readSQL["LEVELA"].ToString() == "1")
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                readSQL.Close();
                conexion.Close();
                return -1;
            }
            catch (SQLiteException ex)
            {
                ex.GetBaseException();
                return -1;
            }
            finally { conexion.Close(); };
        } 
        
        /// <summary>
        /// Me devuelve el nivel del usuario, para comprobar que tiene disponible.
        /// </summary>
        /// <param name="nombreUser"></param>
        /// <returns></returns>
        public string nivelUsuario(string nombreUser)
        {
            conexion.Open();
            comando = new MySqlCommand("SELECT LEVELU from USERS where USER = '"+nombreUser+"'", conexion);
            readSQL = comando.ExecuteReader();

            if (readSQL.Read())
            {
                string nivelUser = readSQL["LEVELU"].ToString();
                conexion.Close();
                return nivelUser;
            }

            readSQL.Close();
            conexion.Close();
            return "";
        }

        /// <summary>
        /// Actualizar un nombre de usuario.
        /// </summary>
        /// <param name="nomACambiar"></param>
        /// <param name="nuevoNom"></param>
        /// <returns></returns>
        public int actualizarNombre(string nomACambiar, string nuevoNom)
        {
            comando = new MySqlCommand("UPDATE USERS set USER='" + nuevoNom + "' WHERE USER='" + nomACambiar + "'", conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            conexion.Close();
            return num;
        }

        /// <summary>
        /// Para actualizar el nivel de un usuario X.
        /// </summary>
        /// <param name="nomACambiar"></param>
        /// <param name="nuevoNivel"></param>
        /// <returns></returns>
        public int actualizarNivel(string nomACambiar, string nuevoNivel)
        {
            comando = new MySqlCommand("UPDATE USERS set LEVELU='" + nuevoNivel + "' WHERE USER='" + nomACambiar + "'", conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            conexion.Close();
            return num;
        }

        /// <summary>
        /// Para añadir usuarios nuevos, por defecto no serán administradores.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="contrasena"></param>
        /// <param name="nivelUser"></param>
        /// <returns></returns>
        public int addUsuarios(string nombre, string contrasena, string nivelUser)
        {
            string fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");

            string hash = BCrypt.Net.BCrypt.HashPassword(contrasena);

            // Comprobar si el usuario ya existe en la base de datos
            string sql = "SELECT COUNT(*) AS cuenta FROM `USERS` WHERE `USER` = @nombre";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) // Si tiene filas, hay un usuario con ese nombre.
                        {
                            reader.Close();
                            return -1;
                        }
                        else
                        {
                            string sql2 = "INSERT INTO USERS (`USER`, `PASS`, `ULT_VISITA`, `LEVELU`, `LEVELA`) " +
                                "VALUES ('@nombre', '@hash', '@fecha', " + nivelUser + ", 0)";
                            using (MySqlCommand cmd2 = new MySqlCommand(sql2, c))
                            {
                                cmd2.Parameters.AddWithValue("@nombre", nombre);
                                cmd2.Parameters.AddWithValue("@hash", hash);
                                cmd2.Parameters.AddWithValue("@fecha_actual", fecha_actual);
                                if (cmd2.ExecuteNonQuery() == 1)
                                {
                                    return 1;
                                } else
                                {
                                    return -1;
                                }
                            }
                        } // else.
                    } // USING READER
                }// USING CMD 1
            } // USING DE CONEXIÓN
        } // añadir Usuarios.

        /// <summary>
        /// Método para poder eliminar los usuarios que queramos.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="admin"></param>
        /// <param name="nivelUser"></param>
        /// <returns></returns>
        public int eliminarUsuarios(string nombre, string nivelUser)
        {
            string sql = "DELETE FROM USERS WHERE USER='" + nombre + "' and LEVELU=" + nivelUser;
            comando = new MySqlCommand(sql, conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            conexion.Close();
            return num;
        }

        /// <summary>
        /// Para actualizar el nivel de Adminsitrador.
        /// </summary>
        /// <param name="nomACambiar"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        public int actualizarAdmin(string nomACambiar, string admin)
        {
            comando = new MySqlCommand("UPDATE USERS set LEVELA=" + admin + " WHERE USER='" + nomACambiar + "'", conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            conexion.Close();
            return num;
        }

        /// <summary>
        /// Insertar un nuevo LOG en administradores cuando se haga una acción.
        /// </summary>
        /// <param name="acciones"></param>
        /// <param name="nombre"></param>
        /// <param name="numNivel"></param>
        /// <returns></returns>
        public void RegistroLogNuevo(string acciones, string nombre, int numNivel)
        {
            DateTime fechaActual = DateTime.Now;

            // Formato MySQL
            string fecha = fechaActual.ToString("yyyy-MM-dd"); // MM para que sean meses.

            conexion = new MySqlConnection("server=db4free.net;uid=albaroot;pwd=albaroot;database=appfinal");
            conexion.Open();

            comando = new MySqlCommand("INSERT INTO `LOGADMIN` (`USER`, `NIVEL`, `ACCIONES`, `FECHA_ACCIONES`) " +
                "VALUES ('"+nombre+"', "+numNivel+", '"+acciones+"', '"+fecha+"')", conexion);
            comando.ExecuteNonQuery();
        }

        /// <summary>
        /// Saber cuantos números de equipos alguien tiene, el máximo es 5.
        /// </summary>
        /// <returns></returns>
        public string SaberEquipos(string usuario)
        {
            string sql = "SELECT COUNT(ID_EQUIPO) as EQUIPOS FROM EQUIPOSPKM WHERE ID_USER IN (SELECT ID FROM USERS WHERE USER = '"+usuario+"');";
            using(MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        return dr["EQUIPOS"].ToString();
                    }
                }
            }
        } // saberEquipos.

    }
}
