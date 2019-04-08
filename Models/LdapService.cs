using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
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

        public bool Login(string email, string password)
        {
            try
            {
                connection.Bind(email, password);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public List<LdapEntry> Search(string email)
        {
            try
            {
                if (!connection.Bound) connection.Connect(configuration["ldap:url"], 389);
                connection.Bind(configuration["ldap:SystemUser"], configuration["ldap:SystemPassword"]);
            }
            catch (System.Exception)
            {
                throw new System.Exception("Error Connecting to AD.");
            }

            List<LdapEntry> user = new List<LdapEntry>();

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
                user.Add(nextEntry);
            }

            connection.Disconnect();

            return user;
        }

        // Finds out what role(s) (or user group(s)) the person logging in is in
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