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

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Models.SubmissionContext _context;
        private readonly StaffDirectoryContext _staffcontext;

        public IndexModel(SubmissionContext context, StaffDirectoryContext staffcontext){

            _context = context;
            _staffcontext = staffcontext;

        }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public string LoginID { get; set; }
        [BindProperty]
        public string Files { get; set; }

        public void OnGet()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var staff = _staffcontext.Staff.Where((s) => s.Email == email).First();

            FirstName = staff.FName;
            LastName = staff.LName;
            Email = staff.Email;
            LoginID = staff.LoginID;

        }
        [BindProperty]
        public Submission Submission { get; set; }
        
        public async Task<IActionResult> OnPostAsync(IFormFile Files)
        {
            if (!ModelState.IsValid)
                return Page();

             // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Submission.Date = DateTime.Now;
            Submission.Status = 14;
            Submission.RequesterID = User.FindFirst("username").Value;
            Submission.AssignedToID = "200568";
            Submission.AssignedToName = "PKOUTOUL";
            _context.Submissions.Add(Submission);

            // Business logic to store uploaded file
            if (Files == null || Files.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "uploadedFiles",
                        Files.FileName);
            
            // Store file path of uploaded document into database column.
            Submission.Files = path;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await Files.CopyToAsync(stream);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Requester");
        }

    }
}
