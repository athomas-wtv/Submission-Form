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
        public Proposal Proposal { get; set; }
        public IList<Proposal> Proposals { get; set; }
        private readonly ProposalContext _context;

        public CancelRequestModel(ProposalContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            Proposal = await _context.Proposals.FirstOrDefaultAsync(s => s.ID == id);

            return Page();
            // try
            // {
            //     Proposals = await _context.Proposals.ToListAsync();

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

            Proposal = await _context.Proposals.FindAsync(id);

            if (Proposal != null)
            {
                _context.Proposals.Remove(Proposal);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Teamlead");
        }

    }
}
