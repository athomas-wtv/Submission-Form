﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IST_Submission_Form.Models
{
    public partial class Proposals
    {
        public int Id { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterLocation { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime SubmitDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DesiredCompletion { get; set; }
        public byte StatusId { get; set; }
        [NotMapped]
        public Status Status { get; set; }
        public string Istcomments { get; set; }
        public string SubmitterComments { get; set; }
        [NotMapped]
        public Users DeveloperName { get; set; }
        public string AssignedTo { get; set; }
        public string Files { get; set; }
    }
}
