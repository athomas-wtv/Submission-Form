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
        public IList<Submission> Submissions { get; set; }
        private readonly SubmissionContext _context;
        public string LoginID;
        public DeveloperModel(SubmissionContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            string LoginID = User.FindFirst("username").Value;
            Submissions = await _context.Submissions
                                .Where(s => s.AssignedTo == LoginID).ToListAsync();
        }

    }
}
