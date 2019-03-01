using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    // [Authorize(Roles = "ist_Teamleader")]
    public class Edit : PageModel
    {
        [BindProperty]
        public string Assignee { get; set; }
        [BindProperty]
        public IList<Status> StatusCodes { get; set; }
        [BindProperty]
        public byte NewStatusCode { get; set; }
        public IList<Users> Users { get; set; }
        public Proposals Proposal;
        private readonly ISTProjectsContext _ISTProjectsContext;

        public Edit(ISTProjectsContext ISTProjectsContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }
        public SelectList CurrentUsers { get; set; }
        public SelectList Status { get; set; }
        public async Task OnGetAsync(int id)
        {
            StatusCodes = await _ISTProjectsContext.Status
                            .Where(c => c.SortProposals > 0).ToListAsync();
            Proposal = _ISTProjectsContext.Proposals
                            .Where(s => s.Id == id).First();
            Users = await _ISTProjectsContext.Users.ToListAsync();

            Status = new SelectList(StatusCodes, "Id", "StatusDescription");
            CurrentUsers = new SelectList(Users, "NetworkId", "Name");

            NewStatusCode = Proposal.Status;
            Assignee = Proposal.AssignedTo;

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

        [Route("/IST/ProjectDetails/{id}")]
        public IActionResult Cancel(int ID)
        {
            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}
