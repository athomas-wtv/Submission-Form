using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IST_Submission_Form.Pages
{
    [Authorize(Roles = "Information Solutions Team")]
    public class DeveloperModel : PageModel
    {
        public IList<Proposals> Proposals;
        private readonly ISTProjectsContext _ISTProjectsContext;
        public DeveloperModel(ISTProjectsContext ISTProjectsContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }
        
        public async Task OnGetAsync()
        {
            // Getting logged in user's username so that the below query can match/pull all proposals assigned to them
            string Username = User.FindFirst("username").Value;
            
            // Query to pull all proposals assigned to the logged in user/developer
            Proposals = await _ISTProjectsContext.Proposals.Where(p => p.AssignedTo == Username).Include(p => p.Status).ToListAsync();
        }

    }
}
