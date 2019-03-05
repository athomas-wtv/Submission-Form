using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IST_Submission_Form.Pages
{
    public class IfTagHelper : TagHelper
    {

        [HtmlAttributeName("ShowForTeamlead")]
        public bool Teamlead { get; set; }

        [HtmlAttributeName("ShowForDeveloper")]
        public bool Developer { get; set; }
        public IActionContextAccessor _actionAccessor;
        public IfTagHelper(IActionContextAccessor actionAccessor)
        {
            _actionAccessor = actionAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Always strip the outer tag name as we never want <if> to render
            output.TagName = null;
            var user = _actionAccessor.ActionContext.HttpContext.User;

            // The teamleader's button
            Teamlead = user.IsInRole("ist_teamleader") ? true : false;
            Developer = user.IsInRole("Information Solutions Team") ? true : false;
            
            if(Teamlead)
            {
                return;
            }
            else if(Developer)
            {
                return;
            }
            
            
            output.SuppressOutput();
        }
        // public override void Process(TagHelperContext context, TagHelperOutput output)
        // {
        //     // Always strip the outer tag name as we never want <if> to render
        //     output.TagName = null;

        //     if (Include && !Exclude)
        //     {
        //         return;
        //     }
        //     var user = _actionAccessor.ActionContext.HttpContext.User;

        //     output.SuppressOutput();
        // }
    }
}
