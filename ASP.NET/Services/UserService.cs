using Microsoft.AspNet.Identity;
using PortfolioWebApi.Models.Domain;
using PortfolioWebApi.Models.Request;
using PortfolioWebApi.Models.Request.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PortfolioWebApi.Services;

namespace PortfolioWebApi.Services
{
    public class UserService : IUserService
    {
        private IJwtAuthService _authenticationService;

        public UserService(IJwtAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public int AddUser(AddUserRequest data)
        {
            using (var conn = GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.AddUser";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(data.Password);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@Id"].Value;
            }

        }

        public bool LogIn(UserLogInRequest user)
        {
            bool isSuccessful = false;
            UserBase response = Get(user.EmailAddress, user.Password);

            if (response != null)
            {
                _authenticationService.LogIn(response);
                isSuccessful = true;
            }
            return isSuccessful;
        }

        public void LogOut()
        {
            _authenticationService.LogOut();
        }

        public UserBase Get(string email, string password)
        {


            using (var conn = GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.SelectByEmail";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmailAddress", email);

                using (var reader = cmd.ExecuteReader())
                {
                    var authUser = new UserBase();
                    while (reader.Read())
                    {
                        authUser.Email = (string)reader["Email"];
                        authUser.Id = (int)reader["Id"];
                        authUser.Password = (string)reader["Password"];
                    }
                    bool isSuccessful = BCrypt.Net.BCrypt.Verify(password, authUser.Password);
                    if (!isSuccessful)
                    {
                        authUser = null;
                        return authUser;
                    }
                    return authUser;
                }
            }
        }

        // Helper method to create and open a DB connection
        private SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString);
            conn.Open();
            return conn;
        }


    }
}