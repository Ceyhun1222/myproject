using System;
using System.Windows;

namespace ChartManager.Helper
{
    internal class NotifyService : INotifyService
    {
        public void ShowMessage(string message, MessageType messageType)
        {
            MessageBoxImage boxImage = MessageBoxImage.None;
            switch (messageType)
            {
                case MessageType.Error:
                    boxImage = MessageBoxImage.Error;
                    break;
                case MessageType.Warning:
                    boxImage = MessageBoxImage.Warning;
                    break;
                case MessageType.Info:
                    boxImage = MessageBoxImage.Information;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            MessageBox.Show(message, "Chart Manager", MessageBoxButton.OK, boxImage);
        }

        public MessageBoxResult ShowConfirmationMessage(string message)
        {
            return MessageBox.Show(message, "Chart Manager", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}