using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class SendEmailModel : PageModel
    {
        // public async Task<IActionResult> OnGet([FromServices]IFluentEmail email)
        // {
        //     await email
        //         .To("andre.thomas@fayette.kyschools.us","Andre")
        //         .Subject("Someone has Submitted a Proposal.")
        //         .Body("Go to you dashboard to view the new proposal.")
        //         .SendAsync();

        //     return Page();
        // }

        public async Task<IActionResult> OnPostProposalSubmissionNotification([FromServices]IFluentEmail email)
        {
            await email
                .To("andre.thomas@fayette.kyschools.us", "Andre")
                .Subject("Someone has Submitted a Proposal.")
                .Body("Go to you dashboard to view the new proposal.")
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
