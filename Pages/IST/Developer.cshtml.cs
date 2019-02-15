using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IST_Submission_Form.Pages
{
    public class DeveloperModel : PageModel
    {
        public IList<Proposal> Proposals { get; set; }
        private readonly ProposalContext _context;
        public string Username;
        public DeveloperModel(ProposalContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            string Username = User.FindFirst("username").Value;
            Proposals = await _context.Proposals
                                .Where(s => s.AssignedToName == Username).ToListAsync();
        }

    }
}
