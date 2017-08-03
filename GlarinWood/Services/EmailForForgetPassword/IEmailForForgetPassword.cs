using System.Threading.Tasks;

namespace GlarinWood.Services
{
    public interface IEmailForForgetPassword
    {
        Task SendEmailForgetPasswordAsync(string userId, string userName, string code);
 
    }
}