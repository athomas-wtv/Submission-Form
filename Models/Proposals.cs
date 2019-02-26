using System;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public partial class Proposals
    {
        public int Id { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterLocation { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DesiredCompletion { get; set; }
        public byte Status { get; set; }
        public string Istcomments { get; set; }
        public string SubmitterComments { get; set; }
        public string AssignedTo { get; set; }
        public string Files { get; set; }
    }
}
