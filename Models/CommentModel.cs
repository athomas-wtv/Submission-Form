using System;
using System.ComponentModel.DataAnnotations;

namespace IST_Submission_Form.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int SubmissionID { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime CreatedAt { get; set; }
        
    }
}