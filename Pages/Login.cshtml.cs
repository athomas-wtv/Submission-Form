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
        public bool? Result { get; set; } = null;

        public LoginModel(ILdapService authService)
        {
            _authService = authService;
        }
        public async Task<ActionResult> OnPostAsync()
        {
            // Checks to see if the email and password that are entered match up to any email/password combination in the DB using the Login() method
            // and assigning "true" of "false" to a variable
            bool access = _authService.Login(email, password);
            
            // Returns a list of users that have the given email address assigned to them and assigns the list to the users variable.
            IList<LdapEntry> users = _authService.Search(email);

            // Returns the list of group(s) that the person logging in is a member of
            IList<string> groups = _authService.Groups(email);
            
            
            if(access)
            {
                // If access is granted, this block begins to create claims for the cookie to be assigned to the user
                var claims = new List<Claim>
                {
                    // This claim creates a placeholder for the email to be added to the cookie
                    new Claim(ClaimTypes.NameIdentifier, email),
                    // This claim creates a placeholder for the name of the user to be added to the cookie
                    new Claim("username", users.First().getAttribute("sAMAccountName").StringValue),
                };
                
                // Cycling through the list of groups the logged in user is a part of
                foreach(string group in groups)
                {
                    // Adding a claim placeholder for each group the logged in user is a part of
                    claims.Add(new Claim(ClaimTypes.Role, group));
                }

                // Creating a Claims Identity variable that holds the list of claim placeholders created above and the authentication type
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // The principal variable represents the current user and is receiving the identity in the line above
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(principal, authProperties);

                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
