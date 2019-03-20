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
    public class ProjectDetailsBackup : PageModel
    {
        [BindProperty]
        public Proposals Proposals { get; set; }
        public List<Comments> RequesterComments { get; set; }
        public List<Comments> DeveloperComments;
        public Status Status;
        private readonly ISTProjectsContext _ISTProjectsContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;
        public IConfiguration _config { get; }

        public ProjectDetailsBackup(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectoryContext, IConfiguration config)
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
            
            // Retrieving the current user's information
            var LoggedInUser = _StaffDirectoryContext.Staff.AsNoTracking().Where(staff => staff.LoginID == User.FindFirst("username").Value).First();

            // Creating an instance of a comment to add to the db
            Comments Comment = new Comments();

            // Adding values to the comment just created above. These fields are not on the form for users to see and update.
            Comment.ProposalId = Proposals.Id;
            Comment.Commenter = LoggedInUser.LoginID;
            Comment.Comment = Body;
            Comment.DateTime = DateTime.Now;
            Comment.CommentType = "Requester"; // Hardcoded "Requester" bcause only the requester will be able to access this post path
            _ISTProjectsContext.Comments.Add(Comment);

            await _ISTProjectsContext.SaveChangesAsync();
            
            // Set RecipientEmailAddress variable to the email of the person not making the comment
            string RecipientEmailAddress = _config["email:TeamLeaderEmail"] == LoggedInUser.Email ? Proposals.SubmitterEmail : _config["email:TestTeamLeaderEmail"];
            string RecipientName = _config["email:TeamLeaderEmail"] == LoggedInUser.Email ? Proposals.SubmitterName : _config["email:TeamLeaderName"];

            // Send email notification to EmailAddress
            await email
                .To(RecipientEmailAddress, RecipientName)
                .Subject(Proposals.SubmitterName + " posted a comment on the '" + Proposals.Title + "' Proposal! | IST Form")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Pages/IST/EmailTemplates/FromDeveloper.cshtml", new { Name = RecipientName })
                .SendAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

        public async Task<IActionResult> OnPostDeveloperAsync(int ID, string Title, string Body, [FromServices]IFluentEmail email)
        {

            if (!ModelState.IsValid)
                return Page();

            var LoggedInUser = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            Comments Comment = new Comments();
            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.ProposalId = Proposals.Id;
            Comment.Commenter = LoggedInUser.LoginID;
            Comment.Comment = Body;
            Comment.DateTime = DateTime.Now;
            Comment.CommentType = "Developer";
            _ISTProjectsContext.Comments.Add(Comment);

            await _ISTProjectsContext.SaveChangesAsync();

            // Query database to get the assigned developer's email address
            var AssignedToStaff = _StaffDirectoryContext.Staff.Where(s => s.LoginID == Proposals.AssignedTo).First();

            // Set EmailAddress variable to the email of the person not making the comment
            var RecipientEmailAddress = _config["email:TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.Email : _config["email:TestTeamLeaderEmail"];
            var RecipientName = _config["email:TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.FName : _config["email:TeamLeaderName"];
            
            // Send email to Teamleader
            await email
                .To(RecipientEmailAddress, RecipientName)
                .Subject(AssignedToStaff.FName + " posted a comment on the '" + Proposals.Title + "' Proposal! | IST Form")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Pages/IST/EmailTemplates/FromDeveloper.cshtml", new { Name = RecipientName })
                .SendAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        } 
    }

}