using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages.Requester
{
    public class RequesterModel : PageModel
    {
        public IList<Proposals> Proposals { get; set; }
        public IList<Status> Status { get; set; }
        private readonly ISTProjectsContext _context;
        private readonly StaffDirectoryContext _StaffDirectoryContext;

        public RequesterModel(ISTProjectsContext context, StaffDirectoryContext StaffDirectoryContext)
        {
            _context = context;
            _StaffDirectoryContext = StaffDirectoryContext;
        }
        
        public async Task OnGetAsync()
        {
            var name = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();

            Proposals = await _context.Proposals.Where(p => p.SubmittedBy == name.LoginID)
                                                    .OrderByDescending(d => d.SubmitDate)
                                                    .ToListAsync();
            Status = await _context.Status
                            .Where(c => c.SortProposals > 0)
                            .ToListAsync();
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
