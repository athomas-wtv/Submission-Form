using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class TeamleadModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
}
