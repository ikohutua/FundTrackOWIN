using OwinAuthServer.Models;
using OwinAuthServer.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplicationOwin.Models;

namespace WebApplicationOwin.Service
{
    public class UserService
    {
        public User GetUserByCredentials(string login, string password)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                return null;
            }

            var hashedPassword = PasswordHashManager.GetPasswordHash(password);

            string query = @"SELECT u.id, u.email as 'Email', u.Login, u.Password, r.Name as 'Role'  
                             FROM Users as u
                             inner join Membershipes as m
                             
                                 on u.Id = m.UserId
                             inner join Roles as r
                             
                                 on m.RoleId = r.Id
                             where u.Login = @login and u.Password = @password ;";

            User user;
            using (AuthContext db = new AuthContext())
            {
                //Todo: add parametr password
                user = db.Database.SqlQuery<User>(query, new SqlParameter("@login", login), new SqlParameter("@password", hashedPassword)).FirstOrDefault();
            }

            if (user != null)
            {
                user.Password = string.Empty;
            }

            return user;
        }
    }
}