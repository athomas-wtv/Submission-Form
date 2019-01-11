using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class TeamleadModel : PageModel
    {
        public IList<Submission> Submissions { get; set; }
        // public Submission Submissions { get; set; }

        private readonly SubmissionContext _context;

        public TeamleadModel(SubmissionContext context)
        {
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            Submissions = await _context.Submissions.ToListAsync();

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
