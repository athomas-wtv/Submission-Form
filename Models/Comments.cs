using System;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public int ProposalId { get; set; }
        public string Commenter { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }
        public string CommentType { get; set; }
    }
}
