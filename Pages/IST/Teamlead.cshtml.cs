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
    [Authorize(Roles ="ist_TeamLeader")]
    public class TeamleadModel : PageModel
    {
        public IList<Proposals> Proposals { get; set; }
        private readonly ISTProjectsContext _context;
        public TeamleadModel(ISTProjectsContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            if(!User.IsInRole("ist_TeamLeader"))
            {
                return Unauthorized();
            }
            Proposals = await _context.Proposals.OrderByDescending(d => d.SubmitDate).ToListAsync();
            return Page();

            // try
            // {
            //     Proposals = await _context.Proposals.ToListAsync();

            // }
            // catch(SqlException)
            // {
            //     Console.WriteLine("Cannot connect to the server!");
            // }
            // catch
            // {

            // }
        }

        
    }
}
