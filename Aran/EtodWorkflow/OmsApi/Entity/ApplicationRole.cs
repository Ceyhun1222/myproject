using Microsoft.AspNetCore.Identity;

namespace OmsApi.Entity
{
    public class ApplicationRole : IdentityRole<long>
    {

    }

    public enum Roles
    {
        Client,
        Admin        
    }
}