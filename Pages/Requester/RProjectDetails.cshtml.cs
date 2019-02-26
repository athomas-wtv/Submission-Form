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
    public class RProjectDetails : PageModel
    {
        [BindProperty]
        public Proposals Proposal { get; set; }
        [BindProperty]
        public List<Comments> RequesterComments { get; set; }
        private readonly ISTProjectsContext _ISTProjectsContext;
        private readonly StaffDirectoryContext _StaffDirectory;

        public RProjectDetails(ISTProjectsContext ISTProjectsContext, StaffDirectoryContext StaffDirectory)
        {
            _ISTProjectsContext = ISTProjectsContext;
            _StaffDirectory = StaffDirectory;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proposal = await _ISTProjectsContext.Proposals.FirstOrDefaultAsync(m => m.Id == id);
            RequesterComments = await _ISTProjectsContext.Comments.Where(c => c.Commenter == Proposal.SubmittedBy && c.ProposalId == Proposal.Id).ToListAsync();

            if (Proposal == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int ID, string Body)
        {
            if (!ModelState.IsValid)
                return Page();
            
            var name = _StaffDirectory.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            Comments Comment = new Comments();

            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.ProposalId = Proposal.Id;
            Comment.Commenter = name.LoginID;
            // Comment.Commenter = name.FName + " " + name.LName;
            Comment.Comment = Body;
            // Comment.CreatedByID = name.EmployeeID;
            Comment.DateTime = DateTime.Now;
            Comment.CommentType = "Requester";
            _ISTProjectsContext.Comments.Add(Comment);

            await _ISTProjectsContext.SaveChangesAsync();

            return RedirectToPage("RProjectDetails", new { ID = ID });
        }
    }
}