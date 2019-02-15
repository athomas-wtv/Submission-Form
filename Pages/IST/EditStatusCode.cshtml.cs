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
        public IList<StatusCodes> StatusCodes { get; set; }
        public Proposal Proposal;
        [BindProperty]
        public byte NewStatusCode { get; set; }
        private readonly StatusCodesContext _StatusCodeContext;
        private readonly ProposalContext _ProposalContext;

        public EditStatusCode(StatusCodesContext StatusCodeContext, ProposalContext ProposalContext)
        {
            _StatusCodeContext = StatusCodeContext;
            _ProposalContext = ProposalContext;
        }
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _StatusCodeContext.StatusCode
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Proposal = _ProposalContext.Proposals
                            .Where(s => s.ID == id).First();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Proposal = _ProposalContext.Proposals
                            .Where(s => s.ID == id).First();
            Proposal.Status = NewStatusCode;
            await _ProposalContext.SaveChangesAsync();

            return RedirectToPage("Teamlead");

        }
    }
}