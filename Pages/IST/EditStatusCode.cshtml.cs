using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class EditStatusCode : PageModel
    {
        public IList<Status> Status { get; set; }
        public Proposals Proposal;
        [BindProperty]
        public byte NewStatusCode { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;

        public EditStatusCode(ISTProjectsContext ISTProjectsContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }
        public async Task OnGetAsync(int id)
        {
            Status = await _ISTProjectsContext.Status
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Proposal = _ISTProjectsContext.Proposals
                            .Where(s => s.Id == id).First();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Proposal = _ISTProjectsContext.Proposals
                            .Where(s => s.Id == id).First();
            Proposal.Status = NewStatusCode;
            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("Teamlead");

        }
    }
}