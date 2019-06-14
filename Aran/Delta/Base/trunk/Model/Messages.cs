using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Aran.Delta.Model
{
    public class Messages
    {
        public static void Info(string message)
        {
            MessageBox.Show(message,"Delta",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        public static void Error(string message)
        {
            MessageBox.Show(message, "Delta", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Warning(string message)
        {
           MessageBox.Show(message, "Delta", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static MessageBoxResult WarningWithResult(string message)
        {
            return MessageBox.Show(message, "Delta", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }

        public static MessageBoxResult Question(string message)
        {
           return MessageBox.Show(message, "Delta", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

    }
}
