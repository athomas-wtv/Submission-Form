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
        public IList<Submission> Submissions { get; set; }
        public IList<StatusCodes> StatusCodes { get; set; }
        private readonly SubmissionContext _context;
        private readonly StatusCodesContext _StatusCodesContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;

        public RequesterModel(SubmissionContext context, StatusCodesContext StatusCodesContext, StaffDirectoryContext StaffDirectoryContext)
        {
            _context = context;
            _StatusCodesContext = StatusCodesContext;
            _StaffDirectoryContext = StaffDirectoryContext;
        }
        
        public async Task OnGetAsync()
        {
            var name = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();

            Submissions = await _context.Submissions.Where(s => s.RequesterID == name.EmployeeID)
                                                    .OrderByDescending(d => d.Date)
                                                    .ToListAsync();
            StatusCodes = await _StatusCodesContext.StatusCode
                            .Where(c => c.SortProposals > 0)
                            .ToListAsync();
            // try
            // {
            //     Submissions = await _context.Submissions.ToListAsync();

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
