using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    // [Authorize(Roles = "ist_TeamLeader")]
    public class EditAssignee : PageModel
    {
        public Proposals Proposal;
        [BindProperty]
        public string Assignee { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;

        public EditAssignee(ISTProjectsContext ISTProjectsContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Proposal = _ISTProjectsContext.Proposals
                            .Where(s => s.Id == id).First();
            Proposal.AssignedTo = Assignee;
            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails/", id);
            // return RedirectToPage("ProjectDetails", new { id = id });

        }
    }
}