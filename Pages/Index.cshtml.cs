using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {
            // Checks the user roles and routes the logged in user to the correct page
            if(!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }
            else if(User.IsInRole("Ist_TeamLeader"))
            {
                return Redirect("/IST/Teamlead");
            }
            else if(User.IsInRole("Information Solutions Team"))
            {
                return Redirect("/IST/Developer");
            }
            else
            {
                return Redirect("/Requester/Requester");
            }

        }
    }
}
