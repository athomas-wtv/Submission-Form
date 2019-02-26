using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IST_Submission_Form.Models;

namespace IST_Submission_Form.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ISTProjectsContext _context;

        public DeleteModel(ISTProjectsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Proposals Proposal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proposal = await _context.Proposals.FirstOrDefaultAsync(m => m.Id == id);

            if (Proposal == null)
            {
                return NotFound();
            }
            return Page();
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
