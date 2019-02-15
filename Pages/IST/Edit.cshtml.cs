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
        public byte NewStatusCode { get; set; }
        public IList<StatusCodes> StatusCodes { get; set; }
        public Proposal Proposal;
        private readonly StatusCodesContext _StatusCodesContext;
        private readonly ProposalContext _ProposalContext;

        public Edit(StatusCodesContext StatusCodesContext, ProposalContext ProposalContext)
        {
            _StatusCodesContext = StatusCodesContext;
            _ProposalContext = ProposalContext;
        }
        
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _StatusCodesContext.StatusCode
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Proposal = _ProposalContext.Proposals
                            .Where(s => s.ID == id).First();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Proposal = _ProposalContext.Proposals
                            .Where(s => s.ID == id).First();

            Proposal.AssignedToName = Assignee;
            Proposal.Status = NewStatusCode;
            await _ProposalContext.SaveChangesAsync();
            return RedirectToPage("ProjectDetails", new { id = id });

        }

        [Route ("/IST/ProjectDetails/{id}")]
        public IActionResult Cancel(int ID)
        {
            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}