using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class EditAssignee : PageModel
    {
        public Proposal Proposal;
        [BindProperty]
        public string Assignee { get; set; }
        private readonly StatusCodesContext _StatusCodeContext;
        private readonly ProposalContext _ProposalContext;

        public EditAssignee(StatusCodesContext StatusCodeContext, ProposalContext ProposalContext)
        {
            _StatusCodeContext = StatusCodeContext;
            _ProposalContext = ProposalContext;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Proposal = _ProposalContext.Proposals
                            .Where(s => s.ID == id).First();
            Proposal.AssignedToName = Assignee;
            await _ProposalContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails/", id);
            // return RedirectToPage("ProjectDetails", new { id = id });

        }
    }
}