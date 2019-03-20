using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;

namespace IST_Submission_Form.Pages
{
    // [Authorize(Roles = "ist_TeamLeader")]
    public class DeleteModel : PageModel
    {
        private readonly ISTProjectsContext _context;

        public DeleteModel(ISTProjectsContext context)
        {
            _context = context;
        }

        // Property used to access the selected Proposal for deletion
        public Proposals Proposal;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check to see if requested Proposal exists. If not, produce 404 error.
            if (id == null)
            {
                return NotFound();
            }
            
            // Query the db for requested proposal matching the id in the URL and the proposal's id
            Proposal = await _context.Proposals.FirstOrDefaultAsync(m => m.Id == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Proposals Proposal)
        {
            if(!ModelState.IsValid)
                return NotFound();
            
            // Removing proposal from the db and saving the changes
            _context.Proposals.Remove(Proposal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Teamlead");
        }
       
    }
}
