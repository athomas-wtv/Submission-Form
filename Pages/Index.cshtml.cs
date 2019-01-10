using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IST_Submission_Form.Models.SubmissionContext _context;
        public IndexModel(IST_Submission_Form.Models.SubmissionContext context){

            _context = context;

        }
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

        public void OnGet()
        {

        }
        [BindProperty]
        public Submission Submission { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Submission.Date = DateTime.Now;
            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
