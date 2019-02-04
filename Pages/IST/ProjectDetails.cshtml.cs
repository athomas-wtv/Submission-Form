using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class ProjectDetails : PageModel
    {
        public Submission Submission { get; set; }
        [BindProperty]
        public Comment Comment { get; set; }
        public List<Comment> Comments { get; set; }
        private readonly SubmissionContext _SubmissionContext;
        private readonly CommentContext _CommentContext;

        public ProjectDetails(SubmissionContext SubmissionContext, CommentContext CommentContext)
        {
            _SubmissionContext = SubmissionContext;
            _CommentContext = CommentContext;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Submission = await _SubmissionContext.Submissions.FirstOrDefaultAsync(m => m.ID == id);
            Comments = await _CommentContext.Comments.ToListAsync();

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
            Comment.CreatedAt = DateTime.Now;
            _CommentContext.Comments.Add(Comment);

            await _CommentContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new {ID = id});
        }
    }
}