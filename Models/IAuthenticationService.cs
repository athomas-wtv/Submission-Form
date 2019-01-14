
namespace IST_Submission_Form.Models
{
    public interface IAuthenticationService
    {
        Staff Login(string email, string password);
    }
}