using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public class Submission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LoginID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "text")]
        public string ProjectDescription { get; set; }
        public string Goal { get; set; }
        [Column("Timeline")]
        public string DesiredCompletionDate { get; set; }
        public int Status { get; set; }
        public string Files { get; set; }
        public string AssignedTo { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}