using System;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public partial class CommentsTicket
    {
        public int Id { get; set; }
        public int Ticket { get; set; }
        public int Commenter { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }

        public CommentsTicket IdNavigation { get; set; }
        public CommentsTicket InverseIdNavigation { get; set; }
    }
}
