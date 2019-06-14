using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.ViewModels.UserViewModels
{
    public class UserWithRoleViewModel
    {
        public ApplicationUser User { get; set; }

        public Roles Role { get; set; }
    }
}
