using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IST_Submission_Form.Models.SubmissionContext _context;
        private readonly StaffDirectoryContext _staffcontext;

        public IndexModel(IST_Submission_Form.Models.SubmissionContext context, StaffDirectoryContext staffcontext){

            _context = context;
            _staffcontext = staffcontext;

        }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public string LoginID { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string ProjectDescription { get; set; }
        [BindProperty]
        public string Goal { get; set; }
        [BindProperty]
        public string DesiredCompletionDate { get; set; }
        [BindProperty]
        public string Location { get; set; }

        public void OnGet()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var staff = _staffcontext.Staff.Where((s) => s.Email == email).First();

            FirstName = staff.FName;
            LastName = staff.LName;
            Email = staff.Email;
            LoginID = staff.LoginID;

        }
        [BindProperty]
        public Submission Submission { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Submission.Date = DateTime.Now;
            Submission.Status = 14;
            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
