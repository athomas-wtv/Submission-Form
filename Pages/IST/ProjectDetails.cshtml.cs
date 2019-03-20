using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IST_Submission_Form.Pages
{
    public class ProjectDetails : PageModel
    {
        [BindProperty]
        public Proposals Proposals { get; set; }
        public List<Comments> RequesterComments { get; set; }
        public List<Comments> DeveloperComments;
        public Status Status;
        private readonly ISTProjectsContext _ISTProjectsContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;
        public IConfiguration _config { get; }

        public ProjectDetails(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectoryContext, IConfiguration config)
        {
            _ISTProjectsContext = ISTProjectsContext;
            _StaffDirectoryContext = StaffDirectoryContext;
            _config = config;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Checking to see if there exists any proposals in the db's proposal's table and, if not, to redirect them to the 404 page.
            var AllProposals = await _ISTProjectsContext.Proposals.Where(p => p.Id == id).ToListAsync();
            if (AllProposals.Count == 0)
                return NotFound();

            // Retrieves the selected proposal by matching the IDs
            Proposals = await _ISTProjectsContext.Proposals.FirstAsync(m => m.Id == id);

            // Retrieves the comments in the "Requester Comment" box. This also includes the Teamleader's comments to the requester
            RequesterComments = await _ISTProjectsContext.Comments.Where(c => c.ProposalId == Proposals.Id && c.CommentType == "Requester").ToListAsync();
            
            // Retrieves the comments in the "Developer Comment" box. This also includes the Teamleader's comments to the developer
            DeveloperComments = await _ISTProjectsContext.Comments.Where(c => c.ProposalId == Proposals.Id && c.CommentType == "Developer").ToListAsync();

            // Retrieves the status so that the View can present the description of the status. Without this, it only shows the code (number).
            Status = await _ISTProjectsContext.Status.Where(s => s.Id == Proposals.StatusId).FirstAsync();
            
            return Page();
        }

        public async Task<IActionResult> OnPostRequesterAsync(int ID, string Title, string Body, [FromServices]IFluentEmail email)
        {
            if (!ModelState.IsValid)
                return Page();
            
            // Adding values to the comment just created above. These fields are not on the form for users to see and update.
            AssignCommentProperties(Body, "Requester", email); // Hardcoded "Requester" because only the requester will be able to access this post path
            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

        public async Task<IActionResult> OnPostDeveloperAsync(int ID, String Title, String Body, [FromServices]IFluentEmail email)
        {

            if (!ModelState.IsValid)
                return Page();

            var LoggedInUser = _StaffDirectoryContext.Staff.AsNoTracking().Where(staff => staff.LoginID == User.FindFirst("username").Value).First();

            // Call function to add values to fields automatically. These fields are not on the form for users to see and update.
            AssignCommentProperties(Body, "Developer", email); // Hardcoded "Developer" because only the developer will be able to access this post path
            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

        public void SendEmailNotification(String CommentType, [FromServices]IFluentEmail email)
        {
            // These variables will be used to help send the email to the correct person.
            var SendTo = "";
            var Name = "";

            // Retrieving the current user's information
            var LoggedInUser = _StaffDirectoryContext.Staff.AsNoTracking().Where(staff => staff.LoginID == User.FindFirst("username").Value).First();

            if(CommentType == "Developer")
            {
                // Query database to get the assigned developer's email address
                var AssignedToStaff = _StaffDirectoryContext.Staff.Where(s => s.LoginID == Proposals.AssignedTo).First();

                // Assigning variables with developer's contact info so that email goes to them
                // Will be overwritten if the current logged in user is not Pete. The code below will route it to Pete.
                SendTo = AssignedToStaff.Email;
                Name = AssignedToStaff.FName;
            }

            if(CommentType == "Requester")
            {
                // Assigning variables with requester/submitter contact info so that email goes to them
                SendTo = Proposals.SubmitterEmail;
                Name = Proposals.SubmitterName;
            }

            // Set RecipientEmailAddress variable to the email of the person not making the comment
            // The first expressoion checks to see if the logged in person is Pete.
            var RecipientEmailAddress = _config["email:TeamLeaderEmail"] == LoggedInUser.Email ? SendTo : _config["email:TeamLeaderEmail"];
            var RecipientName = _config["email:TeamLeaderEmail"] == LoggedInUser.Email ? Name : _config["email:TeamLeaderName"];

            // Send email to expected recipent
            email
                .To(RecipientEmailAddress, RecipientName)
                .Subject("A comment has been posted to the '" + Proposals.Title + "' Proposal! | IST Form")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Pages/IST/EmailTemplates/FromDeveloper.cshtml", new { Name = RecipientName })
                .SendAsync();
        }
        public void AssignCommentProperties(String Body, String CommentType, [FromServices]IFluentEmail email)
        {
            // Retrieving the current user's information
            var LoggedInUser = _StaffDirectoryContext.Staff.AsNoTracking().Where(staff => staff.LoginID == User.FindFirst("username").Value).First();

            // Creating an instance of a comment to add to the db
            Comments Comment = new Comments();
            Comment.ProposalId = Proposals.Id;
            Comment.Commenter = LoggedInUser.LoginID;
            Comment.CommentType = CommentType;
            Comment.Comment = Body;
            Comment.DateTime = DateTime.Now;
            _ISTProjectsContext.Comments.Add(Comment);

            // Calls function to send email notification
            SendEmailNotification(Comment.CommentType, email);
        }
    }
}