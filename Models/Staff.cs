using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Novell.Directory.Ldap;

namespace IST_Submission_Form.Models
{
    [Table("EmailDirectory")]
    public class Staff
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string LoginID { get; set; }
        public string EmployeeID { get; set; }


    }
}