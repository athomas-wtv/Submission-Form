using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class EditAssignee : PageModel
    {
        public Submission Submission;
        [BindProperty]
        public string Assignee { get; set; }
        private readonly StatusCodesContext _StatusCodeContext;
        private readonly SubmissionContext _SubmissionContext;

        public EditAssignee(StatusCodesContext StatusCodeContext, SubmissionContext SubmissionContext)
        {
            _StatusCodeContext = StatusCodeContext;
            _SubmissionContext = SubmissionContext;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Submission = _SubmissionContext.Submissions
                            .Where(s => s.ID == id).First();
            Submission.AssignedToName = Assignee;
            await _SubmissionContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails/", id);
            // return RedirectToPage("ProjectDetails", new { id = id });

        }
    }
}