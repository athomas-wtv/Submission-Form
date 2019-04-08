using System.Threading.Tasks;
using FluentEmail.Core;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IST_Submission_Form.Pages.Requester
{
    public class CancelRequestModel : PageModel
    {
        public Proposals Proposal;
        private readonly ISTProjectsContext _context;
        public IConfiguration _config;

        public CancelRequestModel(ISTProjectsContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            // Retrieve proposal up for deletion
            Proposal = await _context.Proposals.FirstOrDefaultAsync(s => s.Id == id);
            
            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync(int? id, [FromServices]IFluentEmail email)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Proposal != null)
            {
                // Removes proposal from db and saves it
                _context.Proposals.Remove(Proposal);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            SendEmailNotification(email);

            return Redirect("/Requester/Requester");
        }

        public void SendEmailNotification([FromServices]IFluentEmail email)
        {
            // Set variables to Team Leader information so that he receives the email.
            var RecipientEmailAddress = _config["email:TeamLeaderEmail"];
            var RecipientName = _config["email:TeamLeaderName"];

            // Send email to Team Leader about cancellation of proposal
            email
                .To(RecipientEmailAddress, RecipientName)
                .Subject("Requester has cancelled the request for " + Proposal.Title)
                .Body("This is just to notify you that the proposal " + Proposal.Title + " was cancelled by the requester. Reach out to " + Proposal.SubmitterName + " for details.")
                .SendAsync();
        }

    }
}
