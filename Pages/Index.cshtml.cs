using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string ProjectDescription { get; set; }
        [BindProperty]
        public string Goal { get; set; }
        [BindProperty]
        public string Timeline { get; set; }
        public DateTime Date { get; set; }


        public void OnGet()
        {

        }
        public void OnPost()
        {
            
        }
    }
}
