using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace IST_Submission_Form.Models
{
    class LdapService : ILdapService
    {
        private IConfiguration configuration;
        private readonly LdapConnection connection;
        private string searchFilter = "(&(objectClass=person)(objectClass=user)(mail={0}))";


        public LdapService(IConfiguration configuration)
        {
            this.configuration = configuration;
            connection = new LdapConnection();
            connection.Connect(configuration["ldap:url"], 389);
        }

        public LdapEntry Login(string email, string password)
        {
            try
            {
                if (!connection.Bound) connection.Bind(email, password);
                return this.Search(email).FirstOrDefault();
            }
            catch (System.Exception)
            {
                throw new System.Exception("Login Failed.");
            }
        }

        public List<LdapEntry> Search(string email)
        {
            try
            {
                if (!connection.Bound) connection.Connect(configuration["AD:Host"], 389);
                connection.Bind(configuration["AD:SystemUser"], configuration["AD:SystemPassword"]);
            }
            catch (System.Exception)
            {
                throw new System.Exception("Error Connectiont to AD.");
            }

            List<LdapEntry> users = new List<LdapEntry>();

            var result = connection.Search(
                    "DC=fayette,DC=ketsds,DC=net",
                    LdapConnection.SCOPE_SUB,
                    string.Format(searchFilter, email),
                    null,
                    false
                );

            while (result.HasMore())
            {
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = result.Next();
                }
                catch (LdapException)
                {
                    continue;
                }
                nextEntry.getAttributeSet();
                users.Add(nextEntry);
            }

            connection.Disconnect();

            return users;
        }

        public List<string> Groups(string email)
        {
            List<LdapEntry> users = this.Search(email);

            if (users.Count() == 0)
            {
                throw new System.Exception("No users found with that email.");
            }
            if (users.Count() > 1)
            {
                throw new System.Exception("Too many users found with the search criteria");
            }

            LdapEntry user = users.FirstOrDefault();

            return user.getAttribute("memberOf").StringValueArray
                .ToList()
                .ConvertAll(g =>
                {
                    var matches = Regex.Match(g, @"^(?:(?<cn>CN=(?<name>[^,]*)),)?(?:(?<path>(?:(?:CN|OU)=[^,]+,?)+),)?(?<domain>(?:DC=[^,]+,?)+)$");
                    var match = matches.Groups.Where(mg => mg.Name == "name").FirstOrDefault();
                    return match.Value;
                });
        }

        public bool IsMemberOf(string email, string group)
        {
            List<LdapEntry> users = this.Search(email);

            if (users.Count() == 0)
            {
                throw new System.Exception("No users found with that email.");
            }
            if (users.Count() > 1)
            {
                throw new System.Exception("Too many users found with the search criteria");
            }

            LdapEntry user = users.First();
            string[] groups = user.getAttribute("memberOf").StringValueArray;

            return groups.Where(g => g.Contains(group)).Any();
        }
    }
    }