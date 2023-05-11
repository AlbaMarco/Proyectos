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
using System.Text.Json;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Controls;
using AppDI.Pags.PanelAdmin;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

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
                        conexion.Close();
                        return false;
                    }
                } 
                else
                {
                    conexion.Close();
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

                //conexion.Open();
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
            //conexion.Open();
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

        /// <summary>
        /// Necesario para saber el ID del usuaro y almacenarlo correctamente segun ID de la tabla users.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string SaberID(string usuario)
        {
            string sql = "SELECT ID FROM USERS WHERE USER = '" + usuario + "';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        return dr["ID"].ToString();
                    }
                }
            }
        } // saberID.

        /// <summary>
        /// Método que inserta un nuevo JSON que está en una lista de strin. Usado en crear equipos.
        /// </summary>
        /// <param name="listaJSON"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int insertarJsonNuevoEquipo(List<string> listaJSON, string usuario)
        {
            string idUser = SaberID(usuario);
            string numEquipo = SaberEquipos(usuario);

            string json = listaJSON[0] + listaJSON[1] + listaJSON[2] + listaJSON[3] + listaJSON[4] + listaJSON[5];
            string sql = "INSERT INTO `EQUIPOSPKM` (`ID_USER`, `ID_EQUIPO`, `POKEMONS`, `ENFRENTAMIENTOS_JUGADOS`, `ENFRENTAMIENTOS_GANADOS`, `ENFRENTAMIENTOS_PERDIDOS`) " +
                "VALUES ("+idUser+", "+(numEquipo+1)+", '"+json+"', NULL, NULL, NULL)";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        } // insertarJsonNuevoEquipo.

        /// <summary>
        /// Método que sirve para ver los equipos creados de un usuario que se deberá pasar por parámetro.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<JsonDocument> verEquipos(string usuario)
        {
            List<JsonDocument> lista = new List<JsonDocument>();
            string sql = "SELECT POKEMONS FROM EQUIPOSPKM WHERE ID_USER IN (SELECT ID FROM USERS WHERE USER = '" + usuario + "');";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            lista.Add(JsonDocument.Parse(dr["POKEMONS"].ToString()));
                        }
                        return lista;
                    }
                }
            }
        } // verEquipos.

        /// <summary>
        /// Saber cuantos números de equipos que están creados administrativamente.
        /// </summary>
        /// <returns></returns>
        public string SaberEquiposAdmin()
        {
            string sql = "SELECT COUNT(ID_EQUIPO_ALEATORIO) as EQUIPOS FROM EQUIPOS_ALEATORIOS;";
            using (MySqlConnection c = new MySqlConnection(Conex))
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
        } // saberEquiposAdmin.

        /// <summary>
        /// Método que inserta un nuevo JSON que está en una lista de string. Usado en crear equipos de administrador..
        /// </summary>
        /// <param name="listaJSON"></param>
        /// <returns></returns>
        public int insertarJsonNuevoEquipoAdmin(List<string> listaJSON)
        {
            string json = listaJSON[0] + listaJSON[1] + listaJSON[2] + listaJSON[3] + listaJSON[4] + listaJSON[5];
            string sql = "INSERT INTO `EQUIPOS_ALEATORIOS` (`ID_EQUIPO_ALEATORIO`, `POKEMONS`, `ENFRENTAMIENTOS_JUGADOS`, `ENFRENTAMIENTOS_GANADOS`, `ENFRENTAMIENTOS_PERDIDOS`) " +
                "VALUES (NULL, '"+json+"', NULL, NULL, NULL)";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        } // insertarJsonNuevoEquipoAdmin.

        /// <summary>
        /// Método que sirve para ver los equipos creados de un usuario que se deberá pasar por parámetro.
        /// </summary>
        /// <returns></returns>
        public List<JsonDocument> verEquiposAdmin()
        {
            List<JsonDocument> lista = new List<JsonDocument>();
            string sql = "SELECT POKEMONS FROM EQUIPOS_ALEATORIOS;";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            lista.Add(JsonDocument.Parse(dr["POKEMONS"].ToString()));
                        }
                        return lista;
                    }
                }
            }
        } // verEquiposAdmin.

        /// <summary>
        /// Ver todos los logs que hay disponibles. Acción de super admin
        /// </summary>
        /// <returns></returns>
        public DataSet selectLogs()
        {
            string sql = "SELECT * FROM LOGADMIN";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    MySqlDataAdapter dad = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dad.Fill(ds, "logs");
                    return ds;
                }
            }
        } // verSelectsLogs.

        /// <summary>
        /// Este método sirve para guardar las tickets que manden para, más tarde, sean visualizados.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public int EnviarSoporte(string texto)
        {
            string sql = "INSERT INTO `SOPORTETECNICO` (`ID_TICKET`, `TXT_TICKET`, `ESTADO`, `FECHA_ENTRADA`) VALUES (NULL, '" + texto + "', 'ENVIADO', CURRENT_TIMESTAMP)";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Select de todos los tickets de soporte.
        /// </summary>
        /// <returns></returns>
        public DataSet selectSoporteTodo()
        {
            string sql = "SELECT ID_TICKET, TXT_TICKET, ESTADO, FECHA_ENTRADA FROM SOPORTETECNICO";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    MySqlDataAdapter dad = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dad.Fill(ds, "soporte");
                    return ds;
                }
            }
        } // verSelectsTodoSoporteTecnico.

        /// <summary>
        /// Select de todos los tickets de soporte dependiendo del estado.
        /// </summary>
        /// <returns></returns>
        public DataSet selectSoporteEstado(string estado)
        {
            string sql = "SELECT ID_TICKET, TXT_TICKET, ESTADO, FECHA_ENTRADA FROM SOPORTETECNICO WHERE ESTADO = '"+estado+"'";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    MySqlDataAdapter dad = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dad.Fill(ds, "sopEstado");
                    return ds;
                }
            }
        } // verSelectsSopTecnicoEstado.

        /// <summary>
        /// Cambiar el estado de un ticket de soporte.
        /// </summary>
        /// <returns></returns>
        public int cambiarEstadoSopTec(string id_ticket, string estado)
        {
            string sql = "UPDATE SOPORTETECNICO SET ESTADO = '"+estado+"' WHERE ID_TICKET = "+id_ticket+"";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        } // CambiarEstadoSoporteTecnico.

        /// <summary>
        /// Consulta que se le pasará un pkm y se baneará para despues ser utilizado y que no dejen utilizarlo en los equipos.
        /// </summary>
        /// <param name="idPkm"></param>
        /// <param name="nomPkm"></param>
        /// <returns></returns>
        public int banearPkm(string idPkm, string nomPkm)
        {
            string sql = "INSERT INTO `PKM_BANEADO` (`ID_PKM`, `NAMEPKM`) VALUES ('"+idPkm+"', '"+nomPkm+"')";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Select de todos los pokemons baneados.
        /// </summary>
        /// <returns></returns>
        public DataSet selectPkmBaneadosTodo()
        {
            string sql = "SELECT * FROM PKM_BANEADO";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    MySqlDataAdapter dad = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dad.Fill(ds, "soporte");
                    return ds;
                }
            }
        } // verSelectPkmBaneadosTodo.

        /// <summary>
        /// Comprobación del pokemon que se añada, si es 0 significará que no está baneado y 1 que si está ban.
        /// </summary>
        /// <param name="pkm"></param>
        /// <returns></returns>
        public int comprobarPkmBan(string pkm)
        {
            string sql = "SELECT NAMEPKM FROM PKM_BANEADO WHERE NAMEPKM = '"+pkm+"';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if(dr["NAMEPKM"].ToString() == pkm)
                        {
                            return 1; // 1 Significará que si hay.
                        }else
                        {
                            return 0; // 0 Significará que no hay.
                        }
                    }
                }
            }
        } // ComprobarPkmBan.

        /// <summary>
        /// Método utilizado para insertar datos de favoritos a un usuario. Se insertará una fila por cada registro que se haga.
        /// Las categorias sirven para decir si es nombre de un pokemon, nombre de un movimiento o nombre de un tipo.
        /// Se busca ampliar para todo tipo de información.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="nombreFavorito"></param>
        /// <param name="img"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public int añadirFavoritos(string usuario, string nombreFavorito, byte[]img, string categoria)
        {
            string sql = "INSERT INTO `FAVORITOS_USERS` (`USER`, `NAME_FAV`, `IMG_FAV`, `CAT_FAV`) VALUES ('" + usuario+"', '"+nombreFavorito+ "', @img_fav, '" + categoria+"')";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    cmd.Parameters.AddWithValue("@img_fav", img);
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Método utilizado para insertar datos de favoritos a un usuario. Se insertará una fila por cada registro que se haga.
        /// PARA AÑADIR SIN IMAGEN.
        /// Las categorias sirven para decir si es nombre de un pokemon, nombre de un movimiento o nombre de un tipo.
        /// Se busca ampliar para todo tipo de información.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="nombreFavorito"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public int añadirFavoritosNoImg(string usuario, string nombreFavorito, string categoria)
        {
            string sql = "INSERT INTO `FAVORITOS_USERS` (`USER`, `NAME_FAV`, `IMG_FAV`, `CAT_FAV`) VALUES ('" + usuario + "', '" + nombreFavorito + "', null, '" + categoria + "')";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Comprobación del favorito para saber si está o no está ya agregado en favoritos para el usuario en cuestión.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="nombreFav"></param>
        /// <returns></returns>
        public int comprobarFavorito(string usuario, string nombreFav)
        {
            List<string> listaAux = new List<string>();
            string sql = "SELECT NAME_FAV FROM FAVORITOS_USERS WHERE USER = '" + usuario + "' AND NAME_FAV = '"+nombreFav+"';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read()) 
                        {
                            listaAux.Add(dr["NAME_FAV"].ToString());
                        }
                        
                        if(listaAux.Count == 0)
                        {
                            return 0; // 0 Significará que no hay.
                        } else { return 1; } // 1 Significará que si hay.
                    }
                }
            }
        } // comprobarFavoritos.

        /// <summary>
        /// Comprobación de todos los favoritos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int comprobarTodosFavorito(string usuario)
        {
            List<string> listaAux = new List<string>();
            string sql = "SELECT NAME_FAV FROM FAVORITOS_USERS WHERE USER = '" + usuario + "';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaAux.Add(dr["NAME_FAV"].ToString());
                        }

                        return listaAux.Count;
                    }
                }
            }
        } // comprobarTodosFavoritos.

        /// <summary>
        /// Comprobación de todos los Pokémon favoritos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<object> leerPkmsFavorito(string usuario)
        {
            List<object> listaAux = new List<object>();
            string sql = "SELECT NAME_FAV, IMG_FAV FROM FAVORITOS_USERS WHERE USER = '" + usuario + "' AND CAT_FAV = '1';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            byte[] imagenBytes = (byte[])dr["IMG_FAV"];
                            MemoryStream memoryStream = new MemoryStream(imagenBytes);
                            BitmapImage bitmap = new BitmapImage();

                            bitmap.BeginInit();
                            bitmap.StreamSource = memoryStream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                            listaAux.Add(new { Imagen = bitmap, NomFavorito = dr["NAME_FAV"].ToString() });
                        }

                        return listaAux;
                    }
                }
            }
        } // LeerPkmsFavoritos.


        /// <summary>
        /// Comprobación de todos los movimientos favoritos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<object> leerMovsFavorito(string usuario)
        {
            List<object> listaAux = new List<object>();
            string sql = "SELECT NAME_FAV, IMG_FAV FROM FAVORITOS_USERS WHERE USER = '" + usuario + "' AND CAT_FAV = '2';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            BitmapImage bt = new BitmapImage();
                            listaAux.Add(new { Imagen = bt, NomFavorito = dr["NAME_FAV"].ToString() });
                        }

                        return listaAux;
                    }
                }
            }
        } // LeerMovsFavoritos.

        /// <summary>
        /// Comprobación de todos los Tipos favoritos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<object> leerTiposFavorito(string usuario)
        {
            List<object> listaAux = new List<object>();
            string sql = "SELECT NAME_FAV, IMG_FAV FROM FAVORITOS_USERS WHERE USER = '" + usuario + "' AND CAT_FAV = '3';";
            using (MySqlConnection c = new MySqlConnection(Conex))
            {
                c.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, c))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            BitmapImage bt = new BitmapImage();
                            listaAux.Add(new { Imagen = bt, NomFavorito = dr["NAME_FAV"].ToString() });
                        }

                        return listaAux;
                    }
                }
            }
        } // LeerTiposFavoritos.

    }
}
