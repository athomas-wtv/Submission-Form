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
using System;

namespace IST_Submission_Form.Pages
{
    [Authorize(Roles = "Information Solutions Team")]
    public class DeveloperModel : PageModel
    {
        public IList<Proposals> Proposals { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;
        public DeveloperModel(ISTProjectsContext ISTProjectsContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }

        public async Task OnGetAsync()
        {
            string Username = User.FindFirst("username").Value;
            Proposals = await _ISTProjectsContext.Proposals.Where(p => p.AssignedTo == Username).Include(p => p.Status).ToListAsync();

        }

    }
}
