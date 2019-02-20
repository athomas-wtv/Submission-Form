using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    [Table("Proposals")]
    public class Proposal : IComparable<Proposal>
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime SubmitDate { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmitterEmail { get; set; }
        public string RequesterID { get; set; }
        public string Title { get; set; }
        public string SubmitterLocation { get; set; }
        [Column("Description", TypeName = "text")]
        public string ProjectDescription { get; set; }
        public string Goal { get; set; }
        [Column("DesiredCompletion")]
        public string DesiredCompletionDate { get; set; }
        public byte Status { get; set; }
        public string Files { get; set; }
        public string AssignedToName { get; set; }
        public ICollection<Comment> ISTComments { get; set; } = new List<Comment>();
        public ICollection<Comment> SubmitterComments { get; set; } = new List<Comment>();

        public int CompareTo(Proposal other)
        {
            return SubmitDate.CompareTo(other.SubmitDate);
        }
    }
}