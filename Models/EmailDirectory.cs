using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IST_Submission_Form.Models
{
    public class EmailDirectory
    {
        public string LoginID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

    }
}