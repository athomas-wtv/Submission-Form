using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages.Requester
{
    public class CancelRequestModel : PageModel
    {
        public Submission Submission { get; set; }
        public IList<Submission> Submissions { get; set; }
        private readonly SubmissionContext _context;

        public CancelRequestModel(SubmissionContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            Submission = await _context.Submissions.FirstOrDefaultAsync(s => s.ID == id);

            return Page();
            // try
            // {
            //     Submissions = await _context.Submissions.ToListAsync();

            // }
            // catch(SqlException)
            // {
            //     Console.WriteLine("Cannot connect to the server!");
            // }
            // catch
            // {

            // }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Submission = await _context.Submissions.FindAsync(id);

            if (Submission != null)
            {
                _context.Submissions.Remove(Submission);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Teamlead");
        }

    }
}
