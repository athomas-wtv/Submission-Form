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

namespace IST_Submission_Form.Pages
{
    // [Authorize(Roles = "Information Solutions Team")]
    public class ProjectDetails : PageModel
    {
        [BindProperty]
        public Proposals Proposals { get; set; }
        public Status Status { get; set; }
        [BindProperty]
        public List<Comments> RequesterComments { get; set; }
        public List<Comments> DeveloperComments { get; set; }
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
            var AllProposals = await _ISTProjectsContext.Proposals.Where(p => p.Id == id).ToListAsync();
            if (AllProposals.Count == 0)
            {
                return NotFound();
                // return Redirect("/Errors/404?message=Project Does Not Exist&code=404 - Project Not Found");
            }

            Proposals = await _ISTProjectsContext.Proposals.FirstOrDefaultAsync(m => m.Id == id);
            RequesterComments = await _ISTProjectsContext.Comments.Where(c => c.ProposalId == Proposals.Id && c.CommentType == "Requester").ToListAsync();
            DeveloperComments = await _ISTProjectsContext.Comments.Where(c => c.ProposalId == Proposals.Id && c.CommentType == "Developer").ToListAsync();
            Status = await _ISTProjectsContext.Status.Where(s => s.Id == Proposals.StatusId).FirstAsync();

            if (DeveloperComments == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRequesterAsync(int ID, string Body, [FromServices]IFluentEmail email)
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
            Comment.CommentType = "Requester";
            _ISTProjectsContext.Comments.Add(Comment);

            await _ISTProjectsContext.SaveChangesAsync();
            
            // Query database to get the assigned developer's email address
            var AssignedToStaff = _StaffDirectoryContext.Staff.Where(s => Proposals.AssignedTo == s.LoginID).First();

            // Set EmailAddress variable to the email of the person not making the comment
            string RecipientEmailAddress = _config["TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.Email : _config["TeamLeader"];
            string RecipientName = _config["TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.FName : _config["TeamLeaderName"];

            // Send email notification to EmailAddress
            await email
                .To(RecipientEmailAddress, RecipientName)
                .Subject("Someone has Commented on your proposal.")
                .Body("Go to you dashboard to view the new comment.")
                .SendAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

        public async Task<IActionResult> OnPostDeveloperAsync(int ID, string Body, [FromServices]IFluentEmail email)
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
            Console.Write("PROPOSAL ASSIGNED TO:" + Proposals.AssignedTo + "<- HERE!!!!!!");
            var AssignedToStaff = _StaffDirectoryContext.Staff.Where(s => s.LoginID == Proposals.AssignedTo).First();

            // Set EmailAddress variable to the email of the person not making the comment
            string RecipientEmailAddress = _config["TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.Email : _config["TestTeamLeaderEmail"];
            string RecipientName = _config["TeamLeaderEmail"] == LoggedInUser.Email ? AssignedToStaff.FName : _config["TeamLeaderName"];

            // Send email to Teamleader
            await email
                .To(RecipientEmailAddress, RecipientName)
                .Subject("Someone has Commented on the proposal you're assigned to.")
                .Body("Comment Reads:" + "<i>" + Body + "</i>" + "Go to you dashboard to view the new comment.")
                .SendAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

        public bool CheckUserRole()
        {
            return User.IsInRole("ist_TeamLeader");
        }
    }

}