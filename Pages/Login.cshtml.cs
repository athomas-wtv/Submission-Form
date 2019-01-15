using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace IST_Submission_Form.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string password { get; set; }
        private readonly ILdapService _authService;
        public LoginModel(ILdapService authService)
        {
            _authService = authService;
        }

        public IActionResult OnPost()
        {
            bool access = _authService.Login(email, password);
            if(access)
            {
                return RedirectToPage("./Index");

            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
