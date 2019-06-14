using System.Threading.Tasks;
using OmsApi.Entity;

namespace OmsApi.Interfaces
{
    public interface ISignInManager
    {
        Task<ApplicationUser> SignIn(string username, string password);

        Task SignOutAsync();
    }
}