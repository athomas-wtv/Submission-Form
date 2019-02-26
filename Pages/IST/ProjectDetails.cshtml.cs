using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;
using System.Linq;

namespace IST_Submission_Form.Pages
{
    public class ProjectDetails : PageModel
    {
        [BindProperty]
        public Proposals Proposals { get; set; }
        [BindProperty]
        public List<Comments> RequesterComments { get; set; }
        public List<Comments> DeveloperComments { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;

        public ProjectDetails(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectoryContext)
        {
            _ISTProjectsContext = ISTProjectsContext;
            _StaffDirectoryContext = StaffDirectoryContext;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proposals = await _ISTProjectsContext.Proposals.FirstOrDefaultAsync(m => m.Id == id);
            RequesterComments = await _ISTProjectsContext.Comments.Where(c => c.Commenter == Proposals.SubmittedBy 
                                                                            && c.ProposalId == Proposals.Id
                                                                            && c.CommentType == "Requester").ToListAsync();
            DeveloperComments = await _ISTProjectsContext.Comments.Where(c => c.Commenter != Proposals.SubmittedBy 
                                                                            && c.ProposalId == Proposals.Id
                                                                            && c.CommentType == "Developer").ToListAsync();
            // RequesterComments = await _ISTProjectsContext.Comments.Where(c => c.Commenter == Proposals.SubmittedBy && c.ProposalID == Proposal.ID).ToListAsync();
            // DeveloperComments = await _ISTProjectsContext.Comments.Where(c => c.CreatedByID != Proposal.RequesterID && c.ProposalID == Proposal.ID).ToListAsync();

            if (Proposals == null)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsyncRequester(int ID, string Body)
        {
            if (!ModelState.IsValid)
                return Page();
            
            var name = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            Comments Comment = new Comments();
            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.ProposalId = Proposals.Id;
            Comment.Commenter = name.LoginID;
            Comment.Comment = Body;
            Comment.DateTime = DateTime.Now;
            Comment.CommentType = "Requester";
            _ISTProjectsContext.Comments.Add(Comment);

            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

        public async Task<IActionResult> OnPostAsyncDeveloper(int ID, string Body)
        {
            if (!ModelState.IsValid)
                return Page();

            var name = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            Comments Comment = new Comments();
            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.ProposalId = Proposals.Id;
            Comment.Commenter = name.LoginID;
            Comment.Comment = Body;
            Comment.DateTime = DateTime.Now;
            Comment.CommentType = "Developer";
            _ISTProjectsContext.Comments.Add(Comment);

            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }
    }
}