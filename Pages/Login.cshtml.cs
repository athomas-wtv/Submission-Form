using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Novell.Directory.Ldap;
using System.Linq;

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

        public bool? Result { get; set; } = null;
        public async Task<ActionResult> OnPostAsync()
        {
            bool access = _authService.Login(email, password);
            IList<LdapEntry> users = _authService.Search(email);
            IList<string> groups = _authService.Groups(email);
            
            if(access)
            {
                Result = true;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim("username", users.First().getAttribute("sAMAccountName").StringValue),
                };
                
                foreach(string group in groups)
                {
                    claims.Add(new Claim(ClaimTypes.Role, group));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(principal, authProperties);

                return RedirectToPage("/Index");
            }
            else
            {
                Result = false;
                return RedirectToPage();
            }
        }
    }
}
