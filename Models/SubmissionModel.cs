using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IST_Submission_Form.Models;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public class Submission
    {
        public int ID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string ProjectDescription { get; set; }
        public string Goal { get; set; }
        public string Timeline { get; set; }
        [NotMapped]
        private List<Comment> Comments { get; }
    }
}