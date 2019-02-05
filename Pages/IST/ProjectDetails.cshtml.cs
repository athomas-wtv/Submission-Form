using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;

namespace IST_Submission_Form.Pages
{
    public class ProjectDetails : PageModel
    {
        [BindProperty]
        public Submission Submission { get; set; }
        [BindProperty]
        public string Body { get; set; }
        [BindProperty]
        public Comment Comment { get; set; }
        [BindProperty]
        public List<Comment> Comments { get; set; }
        private readonly SubmissionContext _SubmissionContext;


        public ProjectDetails(SubmissionContext SubmissionContext)
        {
            _SubmissionContext = SubmissionContext;
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
                return Page();

            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.ID = default(int);
            Comment.SubmissionID = Submission.ID;
            Comment.CreatedBy = "Andre";
            Comment.CreatedAt = DateTime.Now;
            _SubmissionContext.Comments.Add(Comment);

            await _SubmissionContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = id });
        }
    }
}