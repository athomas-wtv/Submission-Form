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
        private readonly StatusCodesContext _StatusCodesContext;
        private readonly SubmissionContext _SubmissionContext;

        public Edit(StatusCodesContext StatusCodesContext, SubmissionContext SubmissionContext)
        {
            _StatusCodesContext = StatusCodesContext;
            _SubmissionContext = SubmissionContext;
        }
        
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _StatusCodesContext.StatusCode
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Submission = _SubmissionContext.Submissions
                            .Where(s => s.ID == id).First();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Submission = _SubmissionContext.Submissions
                            .Where(s => s.ID == id).First();

            Submission.AssignedToName = Assignee;
            Submission.Status = NewStatusCode;
            await _SubmissionContext.SaveChangesAsync();
            return RedirectToPage("ProjectDetails", new { id = id });

        }

        [Route ("/IST/ProjectDetails/{id}")]
        public IActionResult Cancel(int ID)
        {
            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}