using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISTProjectsContext _istprojectscontext;
        private readonly StaffDirectoryContext _staffcontext;

        public IndexModel(ISTProjectsContext istprojectscontext, StaffDirectoryContext staffcontext)
        {

            _istprojectscontext = istprojectscontext;
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
        public Proposals Proposal { get; set; }
        
        public async Task<IActionResult> OnPostAsync(IFormFile Files)
        {
            if (!ModelState.IsValid)
                return Page();
            var name = _staffcontext.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var staff = _staffcontext.Staff.Where((s) => s.Email == email).First();

            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Proposal.SubmitDate = DateTime.Now;
            Proposal.StatusId = 14;
            Proposal.SubmittedBy = name.LoginID;
            Proposal.AssignedTo = "PKOUTOUL";
            _istprojectscontext.Proposals.Add(Proposal);

            // Business logic to store uploaded file
            // This first step checks to see if any files have been attached to the form to be uploaded.
            // if (Files == null || Files.Length == 0)
            //     return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "uploadedFiles",
                        Files.FileName);
            
            // Store file path of uploaded document into database column.
            Proposal.Files = path;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await Files.CopyToAsync(stream);
            }

            await _istprojectscontext.SaveChangesAsync();

            return RedirectToPage("/Requester/Requester");
        }

    }
}
