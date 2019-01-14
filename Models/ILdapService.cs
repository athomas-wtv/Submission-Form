using System.Collections.Generic;
using System.Threading.Tasks;
using Novell.Directory.Ldap;

namespace IST_Submission_Form.Models
{
    public interface ILdapService
    {
        LdapEntry Login(string email, string password);
        List<LdapEntry> Search(string email);
        bool IsMemberOf(string email, string group);
        List<string> Groups(string email);
    }
}