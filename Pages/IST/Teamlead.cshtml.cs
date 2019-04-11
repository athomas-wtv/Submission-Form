using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    [Authorize(Roles ="Ist_TeamLeader")]
    public class TeamleadModel : PageModel
    {
        public IList<Proposals> Proposals { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;

        public TeamleadModel(ISTProjectsContext ISTProjectsContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Getting all the proposals in the database and ordering them by most recent requested
                Proposals = await _ISTProjectsContext.Proposals.Include(p => p.DeveloperName).Include(p => p.Status).OrderByDescending(d => d.SubmitDate).ToListAsync();

            }
            catch(SqlException)
            {
                Console.WriteLine("Cannot connect to the server!");
            }
        
            return Page();
        }

        
    }
}
