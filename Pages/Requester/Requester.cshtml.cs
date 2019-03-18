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
        public IList<Proposals> Proposals { get; set; }
        public IList<Status> Status { get; set; }
        public object CodeDescription { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;

        public RequesterModel(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectoryContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
            _StaffDirectoryContext = StaffDirectoryContext;
        }
        
        public async Task OnGetAsync()
        {
            var name = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();

            Proposals = await _ISTProjectsContext.Proposals.Include(p => p.Status)
                                                    .Where(p => p.SubmittedBy == name.LoginID)
                                                    .OrderByDescending(d => d.SubmitDate)
                                                    .ToListAsync();


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
