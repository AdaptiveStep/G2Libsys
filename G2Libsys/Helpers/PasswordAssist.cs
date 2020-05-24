namespace G2Libsys.Helpers
{
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A passwordbox helper class for binding password as securestring
    /// </summary>
    public static class PasswordAssist
    {
        public static readonly DependencyProperty BindablePasswordProperty =
        DependencyProperty.RegisterAttached("BindablePassword",
        typeof(SecureString), typeof(PasswordAssist),
        new FrameworkPropertyMetadata(new SecureString(), OnPasswordPropertyChanged));

        //public static readonly DependencyProperty BindPasswordProperty =
        //DependencyProperty.RegisterAttached("BindPassword",
        //typeof(bool), typeof(PasswordAssist), new PropertyMetadata(false, BindPassword));

        private static readonly DependencyProperty UpdatingPasswordProperty =
        DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool),
        typeof(PasswordAssist));

        //public static void SetBindPassword(DependencyObject dp, bool value)
        //{
        //    dp.SetValue(BindPasswordProperty, value);
        //}

        //public static bool GetBindPassword(DependencyObject dp)
        //{
        //    return (bool)dp.GetValue(BindPasswordProperty);
        //}

        public static string GetBindablePassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BindablePasswordProperty);
        }

        public static void SetBindablePassword(DependencyObject dp, SecureString value)
        {
            dp.SetValue(BindablePasswordProperty, value);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPasswordProperty);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
        DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;
            if (!GetUpdatingPassword(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        //private static void BindPassword(DependencyObject sender,
        //DependencyPropertyChangedEventArgs e)
        //{
        //    if (!(sender is PasswordBox passwordBox))
        //        return;
        //    if ((bool)e.OldValue)
        //    {
        //        passwordBox.PasswordChanged -= PasswordChanged;
        //    }
        //    if ((bool)e.NewValue)
        //    {
        //        passwordBox.PasswordChanged += PasswordChanged;
        //    }
        //}

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetUpdatingPassword(passwordBox, true);
            SetBindablePassword(passwordBox, passwordBox.SecurePassword);
            SetUpdatingPassword(passwordBox, false);
        }
    }
}
