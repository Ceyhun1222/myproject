using System.Windows;

namespace AmdbManager.Helper
{
    public interface INotifyService
    {
        void ShowMessage(string message, MessageType messageType = MessageType.Info);
        MessageBoxResult ShowConfirmationMessage(string message);
    }

    public enum MessageType
    {
        Error,
        Warning,
        Info
    }
}
