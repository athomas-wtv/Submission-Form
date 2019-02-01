using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class Edit : PageModel
    {
        [BindProperty]
        public string Assignee { get; set; }
        [BindProperty]
        public int NewStatusCode { get; set; }
        public IList<StatusCodes> StatusCodes { get; set; }
        public Submission Submission;
        private readonly StatusCodesContext _StatusCodeContext;
        private readonly SubmissionContext _SubmissionContext;

        public Edit(StatusCodesContext StatusCodeContext, SubmissionContext SubmissionContext)
        {
            _StatusCodeContext = StatusCodeContext;
            _SubmissionContext = SubmissionContext;
        }
        
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _StatusCodeContext.StatusCode
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Submission = _SubmissionContext.Submissions
                            .Where(s => s.ID == id).First();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Submission = _SubmissionContext.Submissions
                            .Where(s => s.ID == id).First();

            Submission.AssignedTo = Assignee;
            Submission.Status = NewStatusCode;
            await _SubmissionContext.SaveChangesAsync();
            return RedirectToPage("ProjectDetails", new { id = id });

        }
    }
}