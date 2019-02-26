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
        public Proposals Proposal;
        private readonly StatusCodesContext _StatusCodesContext;
        private readonly ISTProjectsContext _ISTProjectsContext;

        public Edit(StatusCodesContext StatusCodesContext, ISTProjectsContext ISTProjectsContext)
        {
            _StatusCodesContext = StatusCodesContext;
            _ISTProjectsContext = ISTProjectsContext;
        }
        
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _StatusCodesContext.StatusCode
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Proposal = _ISTProjectsContext.Proposals
                            .Where(s => s.Id == id).First();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Proposal = _ISTProjectsContext.Proposals
                            .Where(s => s.Id == id).First();

            Proposal.AssignedTo = Assignee;
            Proposal.Status = NewStatusCode;
            await _ISTProjectsContext.SaveChangesAsync();
            return RedirectToPage("ProjectDetails", new { id = id });

        }

        [Route ("/IST/ProjectDetails/{id}")]
        public IActionResult Cancel(int ID)
        {
            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}