﻿using System;
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

namespace AppDI.Recursos
{
    /// <summary>
    /// Clase de control de mi base de datos.
    /// </summary>
    public class DB // Acceso a la Base de datos.
    {
        private SQLiteConnection conexion;
        private SQLiteCommand comando;
        private SQLiteDataReader reader;

        private SQLiteDataAdapter adaptador;
        private DataSet ds;

        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader readSQL;
        private MySqlDataAdapter adapter;

        private MySqlDataReader readUser;
        /// <summary>
        /// Propiedadad para obtener la conexión.
        /// </summary>
        private string Conex { get; set; }

        private bool esAdmin;
        private bool esSuperAdmin;
        /// <summary>
        /// Propiedad para saber el nivel del usuario conectado.
        /// </summary>
        public string nivelUserConectado { get; set; }
        /// <summary>
        /// Propiedad para obtener el nombre de usuario.
        /// </summary>
        public string NomUser { get; set; }

        /// <summary>
        /// Método de conexción con la base de datos embebida. Además, se almacenará el nombre y si es administrador en propiedades.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool ConectarBD(string user, string pass)
        {
            Conex = "server=db4free.net;uid=albaroot;pwd=albaroot;database=appfinal";
            if (connection != null) { connection.Close(); }

            try
            {
                connection = new MySqlConnection("server=db4free.net;uid=albaroot;pwd=albaroot;database=appfinal");
                connection.Open();

                command = new MySqlCommand("SELECT PASS FROM USERS WHERE USER = @user", connection);
                command.Parameters.AddWithValue("@user", user);
                readUser = command.ExecuteReader();


                if (readUser.Read())
                {
                    if (BCrypt.Net.BCrypt.Verify(pass, readUser["PASS"].ToString()))
                    {
                        readUser.Close();
                        // Las contraseñas coinciden, el usuario ha iniciado sesión exitosamente
                        command = new MySqlCommand("SELECT * FROM USERS WHERE USER = @usuario", connection);
                        command.Parameters.AddWithValue("@usuario", user);

                        readSQL = command.ExecuteReader();

                        if (readSQL.Read())
                        {
                            if (readSQL["LEVELA"].ToString() == "1")
                            {
                                esAdmin = true;
                            }
                            else if (readSQL["LEVELA"].ToString() == "2")
                            {
                                esSuperAdmin = true;
                            }
                            else
                            {
                                esAdmin = false;
                                esSuperAdmin = false;
                            }

                            nivelUserConectado = readSQL["LEVELU"].ToString();
                            NomUser = readSQL["USER"].ToString();
                            readSQL.Close();
                            connection.Close();
                            return true;
                        }

                        readUser.Close();
                        connection.Close();

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


                /*command = new MySqlCommand("SELECT * FROM USERS WHERE USER = @usuario AND PASS = @contraseña", connection);
                command.Parameters.AddWithValue("@usuario", user);
                command.Parameters.AddWithValue("@contraseña", pass);


                readSQL = command.ExecuteReader();

                if (readSQL.Read())
                {
                    if (readSQL["LEVELA"].ToString() == "1")
                    {
                        esAdmin = true;
                    }
                    else if (readSQL["LEVELA"].ToString() == "2")
                    {
                        esSuperAdmin = true;
                    }
                    else
                    {
                        esAdmin = false;
                        esSuperAdmin = false;
                    }

                    nivelUserConectado = readSQL["LEVELU"].ToString();
                    NomUser = readSQL["USER"].ToString();
                    readSQL.Close();
                    connection.Close();
                    return true;
                }

                connection.Close();

                return false; */

            }
            catch (SQLiteException ex)
            {
                ex.GetBaseException();
                return false;
            } finally { connection.Close(); };
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
        /// Por defecto, no será administrador y el nivel será el menor, en este caso el nivel 1.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>
        public int registroUsuarios(string nombre, string contraseña)
        {
            Conex = "Data Source = ../../../Resources/AppDI.db; Version = 3; New = false; Compress = True";
            string sql = "INSERT INTO Usuarios (Nombre, Contraseña, EsAdmin, NivelUser) VALUES ('" + nombre + "', '" + contraseña + "', 0, 1)";
            using SQLiteConnection c = new SQLiteConnection(Conex);
            c.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
            {
                try
                {
                    int num = cmd.ExecuteNonQuery();
                    c.Close();
                    return num;
                }
                catch (SQLiteException error)
                {
                    MessageBox.Show("Error. Ya hay ese nombre en base de datos.");
                    c.Close();
                    return 0;
                }

            }
        } // añadir Usuarios.

        /// <summary>
        /// Select de la tabla Usuarios de todo el contenido, excepto de la contraseña.
        /// </summary>
        /// <returns></returns>
        public DataSet selectTodo()
        {
            conexion = new SQLiteConnection("Data Source = ../../../Resources/AppDI.db; Version = 3; New = false; Compress = True; journal_mode=WAL");
            comando = new SQLiteCommand("SELECT Nombre, EsAdmin, NivelUser from Usuarios", conexion);

            conexion.Open();
            adaptador = new SQLiteDataAdapter(comando);
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
            conexion = new SQLiteConnection("Data Source = ../../../Resources/AppDI.db; Version = 3; New = false; Compress = True");
            comando = new SQLiteCommand("SELECT Nombre from Usuarios", conexion);

            conexion.Open();
            adaptador = new SQLiteDataAdapter(comando);
            ds = new DataSet();

            adaptador.Fill(ds, "users");

            List<string> nombres = new List<string>();
            foreach(DataRow dr in ds.Tables["users"].Rows)
            {
                nombres.Add(dr["Nombre"].ToString());
            }

            conexion.Close();

            return nombres;
        }

        /// <summary>
        /// Método para saber que nivel de administrador es la persona. 
        /// Devolverá 1 si es que es, 0 si no lo es y -1 si da error la consulta.
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public int selectAdmin(string nom)
        {
            try
            {
                comando = new SQLiteCommand("Select EsAdmin from Usuarios where Nombre ='" + nom + "';", conexion);

                conexion.Open();
                reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["EsAdmin"].ToString() == "1")
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                    reader.Close();
                    conexion.Close();
                    return -1;
                }

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
            comando = new SQLiteCommand("SELECT NivelUser from Usuarios where Nombre = '"+nombreUser+"'", conexion);
            reader = comando.ExecuteReader();

            if (reader.Read())
            {
                string nivelUser = reader["NivelUser"].ToString();
                conexion.Close();
                return nivelUser;
            }

            reader.Close();
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
            comando = new SQLiteCommand("UPDATE Usuarios set Nombre='" + nuevoNom + "' WHERE Nombre='" + nomACambiar + "'", conexion);
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
            comando = new SQLiteCommand("UPDATE Usuarios set NivelUser='" + nuevoNivel + "' WHERE Nombre='" + nomACambiar + "'", conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            conexion.Close();
            return num;
        }

        /// <summary>
        /// Para añadir usuarios nuevos, por defecto no serán administradores.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="contraseña"></param>
        /// <param name="nivelUser"></param>
        /// <returns></returns>
        public int addUsuarios(string nombre, string contraseña, string nivelUser)
        {
            string sql = "INSERT INTO Usuarios (Nombre, Contraseña, EsAdmin, NivelUser) VALUES ('" + nombre + "', '" + contraseña + "', 0, " + nivelUser + ")";
            using SQLiteConnection c = new SQLiteConnection(Conex);
                c.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
            {
                try
                {
                    int num = cmd.ExecuteNonQuery();
                    c.Close();
                    return num;
                }
                catch (SQLiteException error)
                {
                    MessageBox.Show("Error. Ya hay ese nombre en base de datos.");
                    c.Close();
                    return 0;
                }

            }
        } // añadir Usuarios.

        /// <summary>
        /// Método para poder eliminar los usuarios que queramos.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="admin"></param>
        /// <param name="nivelUser"></param>
        /// <returns></returns>
        public int eliminarUsuarios(string nombre, string admin, string nivelUser)
        {
            string sql = "DELETE FROM Usuarios WHERE Nombre='" + nombre + "' and EsAdmin=" + admin + " and NivelUser=" + nivelUser;
            comando = new SQLiteCommand(sql, conexion);
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
            comando = new SQLiteCommand("UPDATE Usuarios set EsAdmin=" + admin + " WHERE Nombre='" + nomACambiar + "'", conexion);
            conexion.Open();
            int num = comando.ExecuteNonQuery();
            conexion.Close();
            return num;
        }

    }
}
