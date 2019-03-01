using System;
using System.Collections.Generic;

namespace IST_Submission_Form.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string NetworkId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool? Istmember { get; set; }
    }
}
