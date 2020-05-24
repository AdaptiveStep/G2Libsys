namespace G2Libsys.Helpers
{
    using G2Libsys.Library.Extensions;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A passwordbox helper class for binding password as securestring
    /// </summary>
    public static class PasswordAssist
    {
        /// <summary>
        /// Dependency property that enables binding to PasswordBox as securestring
        /// </summary>
        public static readonly DependencyProperty BindablePasswordProperty =
        DependencyProperty.RegisterAttached("BindablePassword",
        typeof(SecureString), typeof(PasswordAssist),
        new FrameworkPropertyMetadata(new SecureString(), OnPasswordPropertyChanged));

        /// <summary>
        /// Checks if password is currently being updates
        /// </summary>
        private static readonly DependencyProperty UpdatingPasswordProperty =
        DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool),
        typeof(PasswordAssist));

        /// <summary>
        /// Set binding to the input as SecureString
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetBindablePassword(DependencyObject dp, SecureString value)
        {
            dp.SetValue(BindablePasswordProperty, value);
        }

        /// <summary>
        /// Get updating status of PasswordBox
        /// </summary>
        /// <param name="dp">PasswordBox</param>
        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPasswordProperty);
        }

        /// <summary>
        /// Set updating status of PasswordBox
        /// </summary>
        /// <param name="dp">PasswordBox</param>
        /// <param name="value">Updating status</param>
        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPasswordProperty, value);
        }

        /// <summary>
        /// Event handler for PasswordBox Password changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPasswordPropertyChanged(DependencyObject sender,
        DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            // If PasswordBox is not being updated
            // NOTE: Only true when Password is being updated outside of the PasswordBox
            if (!GetUpdatingPassword(passwordBox))
            {
                // Set PasswordBox content to binded property
                // NOTE: Should only be used to clear the PasswordBox
                passwordBox.Password = (e.NewValue as SecureString).Unsecure();
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        /// <summary>
        /// Password changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            // Set is currently being updated to true
            SetUpdatingPassword(passwordBox, true);
            // Update the binding
            SetBindablePassword(passwordBox, passwordBox.SecurePassword);
            // Set is currently being updated to false
            SetUpdatingPassword(passwordBox, false);
        }
    }
}
