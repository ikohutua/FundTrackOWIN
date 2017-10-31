using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Serializer;
using System.Text;

namespace OwinAuthServer
{
    public class SecureTokenFormatter : ISecureDataFormat<AuthenticationTicket>
    {
        // Fields
        private TicketSerializer serializer;

        // Constructors
        public SecureTokenFormatter(string key)
        {
            serializer = new TicketSerializer();
        }

        // ISecureDataFormat<AuthenticationTicket> Members
        public string Protect(AuthenticationTicket ticket)
        {
            var ticketData = serializer.Serialize(ticket);
            var protectedString = Encoding.UTF8.GetString(ticketData);
            return protectedString;
        }

        public AuthenticationTicket Unprotect(string text)
        {
            var protectedData = Encoding.UTF8.GetBytes(text);
            var ticket = serializer.Deserialize(protectedData);
            return ticket;
        }
    }
}
