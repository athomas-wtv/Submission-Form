using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authorization;

namespace IST_Submission_Form.Pages
{
    public class e404Model : PageModel
    {
        public string error { get; set; } = "Not Found";
        public e404Model()
        {
        }

        public void OnGet(string error)
        {
            if(error == "project_not_found")
            {
                error = "Project not found";
            }

        }
    }
}
