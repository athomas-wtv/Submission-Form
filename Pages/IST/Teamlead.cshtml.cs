using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class TeamleadModel : PageModel
    {
        public IList<Proposals> Proposals { get; set; }
        private readonly ISTProjectsContext _context;
        public TeamleadModel(ISTProjectsContext context)
        {
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            Proposals = await _context.Proposals.OrderByDescending(d => d.SubmitDate).ToListAsync();

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
