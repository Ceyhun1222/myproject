using System.Threading.Tasks;

namespace ObstacleManagementSystem.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
