using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IST_Submission_Form.Pages
{
    public class e404Model : PageModel
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet(string message, string code)
        {
            ErrorMessage = message;
            ErrorCode = code;

        }
    }
}
