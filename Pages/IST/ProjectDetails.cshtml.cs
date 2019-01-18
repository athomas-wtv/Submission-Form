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
        private readonly SubmissionContext _context;

        public ProjectDetails(SubmissionContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Submission = await _context.Submissions.FirstOrDefaultAsync(m => m.ID == id);

            if (Submission == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}