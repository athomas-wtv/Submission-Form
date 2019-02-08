using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;
using System.Linq;

namespace IST_Submission_Form.Pages
{
    public class ProjectDetails : PageModel
    {
        [BindProperty]
        public Submission Submission { get; set; }
        [BindProperty]
        public Comment Comment { get; set; }
        [BindProperty]
        public List<Comment> Comments { get; set; }
        private readonly SubmissionContext _SubmissionContext;
        private readonly StaffDirectoryContext _StaffDirectory;

        public ProjectDetails(SubmissionContext SubmissionContext, StaffDirectoryContext StaffDirectory)
        {
            _SubmissionContext = SubmissionContext;
            _StaffDirectory = StaffDirectory;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Submission = await _SubmissionContext.Submissions.FirstOrDefaultAsync(m => m.ID == id);
            Comments = await _SubmissionContext.Comments.ToListAsync();

            if (Submission == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();
            
            var name = _StaffDirectory.Staff.Where(s => s.LoginID == User.FindFirst("username").Value).First();

            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.SubmissionID = Submission.ID;
            Comment.CreatedBy = name.FName + " " + name.LName;
            Comment.CreatedAt = DateTime.Now;
            _SubmissionContext.Comments.Add(Comment);
            Comment.ID = default(int);

            await _SubmissionContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = id });
        }
    }
}