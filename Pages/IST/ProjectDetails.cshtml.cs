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
        public Proposal Proposal { get; set; }
        [BindProperty]
        public List<Comment> RequesterComments { get; set; }
        public List<Comment> DeveloperComments { get; set; }
        private readonly ProposalContext _ProposalContext;
        private readonly StaffDirectoryContext _StaffDirectoryContext;

        public ProjectDetails(ProposalContext ProposalContext, StaffDirectoryContext StaffDirectoryContext)
        {
            _ProposalContext = ProposalContext;
            _StaffDirectoryContext = StaffDirectoryContext;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proposal = await _ProposalContext.Proposals.FirstOrDefaultAsync(m => m.ID == id);
            RequesterComments = await _ProposalContext.Comments.Where(c => c.CreatedByID == Proposal.RequesterID && c.ProposalID == Proposal.ID).ToListAsync();
            DeveloperComments = await _ProposalContext.Comments.Where(c => c.CreatedByID != Proposal.RequesterID && c.ProposalID == Proposal.ID).ToListAsync();

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
            
            var name = _StaffDirectoryContext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            Comment Comment = new Comment();
            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.ProposalID = Proposal.ID;
            Comment.CreatedByName = name.FName + " " + name.LName;
            Comment.Body = Body;
            Comment.CreatedByID = name.EmployeeID;
            Comment.CreatedAt = DateTime.Now;
            _ProposalContext.Comments.Add(Comment);

            await _ProposalContext.SaveChangesAsync();

            return RedirectToPage("ProjectDetails", new { ID = ID });
        }

    }
}