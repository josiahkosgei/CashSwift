﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace CashSwiftDeposit.UserControls
{
    internal class TimeoutDialogBox
    {
        private static Window CreateAutoCloseWindow(TimeSpan timeout)
        {
            Window window1 = new Window();
            window1.WindowStyle = WindowStyle.None;
            window1.WindowState = WindowState.Maximized;
            window1.Background = Brushes.White;
            window1.AllowsTransparency = true;
            window1.Opacity = 0.5;
            window1.ShowInTaskbar = false;
            window1.ShowActivated = true;
            window1.Topmost = true;
            Window window2 = window1;
            window2.Show();
            IntPtr handle = new WindowInteropHelper(window2).Handle;
            Task.Delay((int)timeout.TotalMilliseconds).ContinueWith(t => NativeMethods.SendMessage(handle, 16U, IntPtr.Zero, IntPtr.Zero));
            return window2;
        }

        public static MessageBoxResult ShowDialog(
          string message,
          int timeout,
          string title = null,
          MessageBoxButton messageBoxButton = MessageBoxButton.OK,
          MessageBoxImage messageBoxImage = MessageBoxImage.None,
          MessageBoxResult defaultMessageBoxResult = MessageBoxResult.No)
        {
            if (timeout <= 0)
                return MessageBox.Show(message, title, messageBoxButton);
            Window autoCloseWindow = CreateAutoCloseWindow(TimeSpan.FromSeconds(timeout));
            try
            {
                return MessageBox.Show(autoCloseWindow, message, title, messageBoxButton, messageBoxImage, defaultMessageBoxResult);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                autoCloseWindow.Close();
            }
        }
    }
}
