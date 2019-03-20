using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IST_Submission_Form.Pages
{
    // [Authorize(Roles = "Ist_TeamLeader")]
    public class Edit : PageModel
    {
        // The properties that have the [BindProperty] annotation are showing up on the View
        [BindProperty]
        public string Assignee { get; set; }
        [BindProperty]
        public IList<Status> StatusCodes { get; set; }
        [BindProperty]
        public byte NewStatusCode { get; set; }
        public IList<Users> Users;
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
        
        // Declaring these fields to use as dropdown variables for this page
        public SelectList ISTUsers;
        public SelectList Status;
        public async Task OnGetAsync(int id)
        {
            // Checking to see if current user is Pete to allow him to see all the status codes
            if(User.FindFirst("username").Value == "pkoutoul")
            {
                StatusCodes = await _ISTProjectsContext.Status.Where(c => c.SortProposals > 0).ToListAsync();
            }
            else
            {
                // Creating a short sub-list of the status codes specifically for developers
                // Each number corresponds to a status code in the database
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

            // Grabs the proposal in question (or being edited)
            Proposal = _ISTProjectsContext.Proposals.Where(s => s.Id == id).First();
            
            // Gets a list of all members of IST
            Users = await _ISTProjectsContext.Users.ToListAsync();
            
            // Creating two dropdown lists: one for status' and one for the IST users
            Status = new SelectList(StatusCodes, "Id", "StatusDescription");
            ISTUsers = new SelectList(Users, "NetworkId", "Name");

            // Making sure that the dropdown defaults to the current value
            NewStatusCode = Proposal.StatusId;
            Assignee = Proposal.AssignedTo;

        }

        public async Task<IActionResult> OnPostAsync(int id, [FromServices]IFluentEmail email)
        {
            // Pulls the proposal to edit
            Proposal = _ISTProjectsContext.Proposals.Where(s => s.Id == id).First();

            // Added to make sure that when developers update the status of the proposal, the AssignedTo variable doesn't 
            // get updated to NULL thus unassigning it from the developer. If true, this if statement returns to the 
            // developer's view bypassing the line that updates the Assignee so that it remains the same.
            if(string.IsNullOrEmpty(Assignee))
            {
                // Assigns newly selected status code to proposal, saves, then sends email to the proposal requester/suubmitter
                Proposal.StatusId = NewStatusCode;
                await _ISTProjectsContext.SaveChangesAsync();
                SendEmailNotification(email);
                
                // Redirects to developer because only the developer's actions will be routed to this track
                return RedirectToPage("Developer", new { id = id });
            }
            
            // If the new and old status codes are the same then it wasn't updated. Therefore no notification needs to be sent.
            if(Proposal.StatusId != NewStatusCode)
                SendEmailNotification(email);

            // Assigns the newly selected Assignee and Status Code to the db record and saves it
            Proposal.AssignedTo = Assignee;
            Proposal.StatusId = NewStatusCode;
            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { id = id });
        }

        public void SendEmailNotification([FromServices]IFluentEmail email)
        {
            // HEADING: Logic to send email notification when the status changes.
            // Set variables to the email address and name of the person not making the comment
            string RequesterEmailAddress = Proposal.SubmitterEmail;
            string RequesterName = Proposal.SubmitterName;

            // Send email notification to requester/submitter notifying them that the status of their project has changed
            email
                .To(RequesterEmailAddress, RequesterName)
                .Subject("The Status of Your Project Has Changed! | " + Proposal.Title)
                .Body("Email Sent")
                // .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Pages/IST/EmailTemplates/StatusChange.cshtml", new { Name = RequesterName })
                .SendAsync();
        }

        // For the cancel button to redirect back to project details
        [Route("/IST/ProjectDetails/{id}")]
        public IActionResult Cancel(int ID)
        {
            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}
