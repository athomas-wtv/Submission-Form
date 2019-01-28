using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Novell.Directory.Ldap;

namespace IST_Submission_Form.Models
{
    [Table("Status")]
    public class StatusCodes
    {
        public byte ID { get; set; }
        public string StatusDescription { get; set; }
        public int SortProjects { get; set; }
        public int SortProposals { get; set; }
        public int SortTickets { get; set; }


    }
}