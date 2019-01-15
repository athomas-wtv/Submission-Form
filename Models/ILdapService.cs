using System.Collections.Generic;
using System.Threading.Tasks;
using Novell.Directory.Ldap;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Models
{
    public interface ILdapService
    {
        bool Login(string email, string password);
        List<LdapEntry> Search(string email);
        bool IsMemberOf(string email, string group);
        List<string> Groups(string email);

    }
}