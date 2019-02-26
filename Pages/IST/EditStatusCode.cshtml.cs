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
        public Proposals Proposal;
        [BindProperty]
        public byte NewStatusCode { get; set; }
        private readonly StatusCodesContext _StatusCodeContext;
        private readonly ISTProjectsContext _ISTProjectsContext;

        public EditStatusCode(StatusCodesContext StatusCodeContext, ISTProjectsContext ISTProjectsContext)
        {
            _StatusCodeContext = StatusCodeContext;
            _ISTProjectsContext = ISTProjectsContext;
        }
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _StatusCodeContext.StatusCode
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