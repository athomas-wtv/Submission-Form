using System;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public partial class Status
    {
        public byte Id { get; set; }
        public string StatusDescription { get; set; }
        public int SortProjects { get; set; }
        public int SortProposals { get; set; }
        public int SortTickets { get; set; }
    }
}
