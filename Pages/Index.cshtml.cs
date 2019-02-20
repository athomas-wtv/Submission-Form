using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Models.ProposalContext _proposalcontext;
        private readonly StaffDirectoryContext _staffcontext;

        public IndexModel(ProposalContext proposalcontext, StaffDirectoryContext staffcontext){

            _proposalcontext = proposalcontext;
            _staffcontext = staffcontext;

        }
        [BindProperty]
        public string SubmitterName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public string LoginID { get; set; }
        [BindProperty]
        public string Files { get; set; }

        public void OnGet()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var staff = _staffcontext.Staff.Where((s) => s.Email == email).First();

            SubmitterName = staff.LName + ", " + staff.FName;
            Email = staff.Email;
            LoginID = staff.LoginID;

        }
        [BindProperty]
        public Proposal Proposal { get; set; }
        
        public async Task<IActionResult> OnPostAsync(IFormFile Files)
        {
            if (!ModelState.IsValid)
                return Page();
            var name = _staffcontext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var staff = _staffcontext.Staff.Where((s) => s.Email == email).First();

            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Proposal.SubmitDate = DateTime.Now;
            Proposal.Status = 14;
            Proposal.RequesterID = name.EmployeeID;
            Proposal.AssignedToName = "PKOUTOUL";
            _proposalcontext.Proposals.Add(Proposal);

            // Business logic to store uploaded file
            if (Files == null || Files.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "uploadedFiles",
                        Files.FileName);
            
            // Store file path of uploaded document into database column.
            Proposal.Files = path;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await Files.CopyToAsync(stream);
            }

            await _proposalcontext.SaveChangesAsync();

            return RedirectToPage("/Requester/Requester");
        }

    }
}
