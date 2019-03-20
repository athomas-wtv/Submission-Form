using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {

            if(!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }
            else if(User.IsInRole("Ist_TeamLeader"))
            {
                return Redirect("/IST/Teamlead");
            }
            else if(User.IsInRole("Information Solutions Team") && !User.IsInRole("Ist_TeamLeader"))
            {
                return Redirect("/IST/Developer");
            }
            else if(!User.IsInRole("Information Solutions Team") && !User.IsInRole("Ist_TeamLeader"))
            {
                return Redirect("/Requester/Requester");
            }
            return Redirect("/Errors/404");

        }
    }
}
