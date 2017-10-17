using System.Data.Entity;
using WebApplicationOwin.Models;

namespace OwinAuthServer.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext()
            : base("AuthContext")
        {
        }
    }
}