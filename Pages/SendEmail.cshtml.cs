using System;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace IST_Submission_Form.Pages
{
    public class SendEmailModel : PageModel
    {
        public StaffDirectoryContext _StaffDirectoryContext;
        public IConfiguration _config;
        public Proposals Proposals;
        public ISTProjectsContext _ISTProjectsContext;

        public SendEmailModel(IConfiguration config, StaffDirectoryContext StaffDirectoryContext, ISTProjectsContext ISTProjectsContext)
        {
            _config = config;
            _StaffDirectoryContext = StaffDirectoryContext;
            _ISTProjectsContext = ISTProjectsContext;
        }

        public void OnGet([FromServices]IFluentEmail email)
        {
            // await email
            //     .To("andre.thomas@fayette.kyschools.us","Andre")
            //     .Subject("Someone has Submitted a Proposal.")
            //     .Body("Go to you dashboard to view the new proposal.")
            //     .SendAsync();

            // return Page();

            Proposals = _ISTProjectsContext.Proposals.FirstOrDefault(m => m.Id == 8);

        }

        public async Task<IActionResult> OnPostProposalSubmissionNotification([FromServices]IFluentEmail email)
        {
            // await email
            //     .To("andre.thomas@fayette.kyschools.us", "Andre")
            //     .Subject("Someone has Submitted a Proposal.")
            //     .Body("Go to you dashboard to view the new proposal.")
            //     .SendAsync();
            Proposals = _ISTProjectsContext.Proposals.FirstOrDefault(m => m.Id == 8);

            var LoggedInUser = _StaffDirectoryContext.Staff.Where(s => s.LoginID == User.FindFirst("username").Value).First();

            // Query database to get the assigned developer's email address
            var AssignedToStaff = _StaffDirectoryContext.Staff.Where(s => s.LoginID == Proposals.AssignedTo).First();

            // Set EmailAddress variable to the email of the person not making the comment
            string RecipientEmailAddress = _config["TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.Email : _config["TestTeamLeaderEmail"];
            string RecipientName = _config["TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.FName : _config["TeamLeaderName"];

            Console.Write("Email Renderer: " + email.Renderer + " <- HERE!!!!!!");
            Console.Write("Email Sender: " + email.Sender + " <- HERE!!!!!!");

            // Send email to Teamleader
            await email
                .To(RecipientEmailAddress, RecipientName)
                .Subject("Someone has Commented on the proposal you're assigned to.")
                .Body("Comment Reads: Go to you dashboard to view the new comment.")
                .SendAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCommentNotification([FromServices]IFluentEmail email)
        {
            await email
                .To("andre.thomas@fayette.kyschools.us", "Andre")
                .Subject("Someone has Commented on your proposal.")
                .Body("Go to you dashboard to view the new comment.")
                .SendAsync();

            return Page();
        }
    }
}
