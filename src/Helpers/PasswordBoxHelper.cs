using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RcloneQBController.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(SecureString), typeof(PasswordBoxHelper), new PropertyMetadata(null, OnBoundPasswordChanged));

        public static SecureString GetBoundPassword(DependencyObject d)
        {
            return (SecureString)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, SecureString value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (e.NewValue != null)
                {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.SecurePassword);
            }
        }
    }
}