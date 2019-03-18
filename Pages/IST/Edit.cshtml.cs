using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IST_Submission_Form.Pages
{
    // [Authorize(Roles = "ist_TeamLeader")]
    public class Edit : PageModel
    {
        [BindProperty]
        public string Assignee { get; set; }
        [BindProperty]
        public IList<Status> StatusCodes { get; set; }
        [BindProperty]
        public byte NewStatusCode { get; set; }
        public IList<Users> Users { get; set; }
        public Proposals Proposal;
        private readonly ISTProjectsContext _ISTProjectsContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;
        public IConfiguration _config { get; }


        public Edit(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectoryContext, IConfiguration config)
        {
            _ISTProjectsContext = ISTProjectsContext;
            _StaffDirectoryContext = StaffDirectoryContext;
            _config = config;
        }
        public SelectList CurrentUsers { get; set; }
        public SelectList Status { get; set; }
        public async Task OnGetAsync(int id)
        {
            if(User.FindFirst("username").Value == "pkoutoul")
            {
                StatusCodes = await _ISTProjectsContext.Status.Where(c => c.SortProposals > 0).ToListAsync();
            }
            else
            {
                // Creating a short sub-list of the status codes specifically for developers
                // Each number corresponds to a status code in hte database
                var DevStatusCodes = new List<int>();
                DevStatusCodes.Add(2);
                DevStatusCodes.Add(3);
                DevStatusCodes.Add(5);
                DevStatusCodes.Add(7);
                StatusCodes = await _ISTProjectsContext.Status.Where(c => 
                        c.SortProposals > 0 &&
                        c.Id == 1 ||
                        c.Id == 2 ||
                        c.Id == 13 ||
                        c.Id == 15
                    ).ToListAsync();
            }
            
            Proposal = _ISTProjectsContext.Proposals.Where(s => s.Id == id).First();
            Users = await _ISTProjectsContext.Users.ToListAsync();

            Status = new SelectList(StatusCodes, "Id", "StatusDescription");
            CurrentUsers = new SelectList(Users, "NetworkId", "Name");

            NewStatusCode = Proposal.StatusId;
            Assignee = Proposal.AssignedTo;

        }

        public async Task<IActionResult> OnPostAsync(int id, [FromServices]IFluentEmail email)
        {
            Proposal = _ISTProjectsContext.Proposals.Where(s => s.Id == id).First();
            
            // Added to make sure when developers update the status, the AssignedTo variable doesn't get updated to NULL
            // thus unassigning it from the developer. This if statment returns to the developer's view.
            if(string.IsNullOrEmpty(Assignee))
            {
                Proposal.StatusId = NewStatusCode;
                await _ISTProjectsContext.SaveChangesAsync();
                SendEmailNotification(email);
                return RedirectToPage("Developer", new { id = id });
            }
            
            // If the new and old status codes are the same then it wasn't updated. Therefore no notification needs to be sent.
            if(Proposal.StatusId != NewStatusCode)
            {
                SendEmailNotification(email);
            }

            Proposal.AssignedTo = Assignee;
            Proposal.StatusId = NewStatusCode;
            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { id = id });
        }

        public void SendEmailNotification([FromServices]IFluentEmail email)
        {
            // HEADING: Logic to send email notification when the status changes.
            // SUB-HEADING: Set RequesterEmailAddress variable to the email of the person not making the comment
            // Set requester information into simpler-looking variables
            string RequesterEmailAddress = Proposal.SubmitterEmail;
            string RequesterName = Proposal.SubmitterName;

            // Send email notification to Requester notifying them that the status of their project has changed
            email
                .To(RequesterEmailAddress, RequesterName)
                .Subject("The Status of Your Project Has Changed! | " + Proposal.Title)
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Pages/IST/EmailTemplates/StatusChange.cshtml", new { Name = RequesterName })
                .SendAsync();
        }

        [Route("/IST/ProjectDetails/{id}")]
        public IActionResult Cancel(int ID)
        {
            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}
