using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceAllers.Models;

namespace WebServiceAllers.Dao
{
    public class UsersDao
    {
        public UsuarioModel GetUsuario(string Login, string Password)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            UsuarioModel oUsuI = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand oSQL = new SqlCommand("spLoginUsuario", connection);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.CommandTimeout = 100000;
                oSQL.Parameters.Add(new SqlParameter("@Login", Login));
                oSQL.Parameters.Add(new SqlParameter("@Password", Password));
                connection.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    oUsuI = new UsuarioModel()
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Login = reader["Login"].ToString(),
                        Password = reader["Password"].ToString(),
                        Activo = bool.Parse(reader["Activo"].ToString())
                    };
                }
                connection.Close();
            }
            return oUsuI;
        }
    }
}