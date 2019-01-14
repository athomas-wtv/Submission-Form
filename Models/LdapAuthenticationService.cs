using System;
using IST_Submission_Form.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.Extensions.Options;

namespace IST_Submission_Form.Pages
{
    class LdapAuthenticationService : IAuthenticationService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";

        private readonly LdapConfig _config;
        LdapConnection _connection;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            try
            {
                connection = new LdapConnection();
                connection.Connect(_config.Url, _config.Port);
            }
            catch (LdapException lex)
            {
                Console.WriteLine("CONNECTION ERROR");
                Console.WriteLine("DETAILS - " + lex.ToString());
            }
        }

        public Staff Login(string username, string password)
        {
            _connection.Connect(_config.Url, LdapConnection.DEFAULT_SSL_PORT);
            _connection.Bind(_config.BindDn, _config.BindCredentials);

            var searchFilter = string.Format(_config.SearchFilter, username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
                false
            );

            try
            {
                var staff = result.next();
                if (staff != null)
                {
                    _connection.Bind(staff.DN, password);
                    if (_connection.Bound)
                    {
                        return new Staff
                        {
                            DisplayName = staff.getAttribute(DisplayNameAttribute).StringValue,
                            Username = staff.getAttribute(SAMAccountNameAttribute).StringValue,
                            IsAdmin = staff.getAttribute(MemberOfAttribute).StringValueArray.Contains(_config.AdminCn)
                        };
                    }
                }
            }
            catch
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();
            return null;
        }

        Staff IAuthenticationService.Login(string email, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}