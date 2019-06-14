using Microsoft.AspNetCore.Identity;

namespace ObstacleManagementSystem.Models
{
    public class ApplicationRole : IdentityRole<long>
    {

    }

    public enum Roles
    {
        Client,
        //Moderator,
        Admin,
        Moderator
    }
}