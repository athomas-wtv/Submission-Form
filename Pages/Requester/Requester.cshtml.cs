using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages.Requester
{
    [Authorize]
    public class RequesterModel : PageModel
    {
        public IList<Proposals> Proposals;
        private readonly ISTProjectsContext _ISTProjectsContext;

        public RequesterModel(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectoryContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }
        
        public async Task OnGetAsync()
        {
            // Pulls all the proposals requested by the user logged in and orders them by the most recent requested to the oldest request
            Proposals = await _ISTProjectsContext.Proposals.Include(p => p.Status)
                                                    .Where(p => p.SubmittedBy == User.FindFirst("username").Value)
                                                    .OrderByDescending(d => d.SubmitDate)
                                                    .ToListAsync();
        }

    }
}
