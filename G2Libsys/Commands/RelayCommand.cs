namespace G2Libsys.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// <typeparamref name="ICommand"/> implementation
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        /// <summary>
        /// Initializes command without <paramref name="canExecute"/>
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// Create new command and checks if <paramref name="canExecute"/> returns true
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) 
            : base(execute, canExecute) { }
    }

    /// <summary>
    /// Generic <inheritdoc cref="RelayCommand"/>where <typeparamref name="T"/> is Type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        /// <summary>
        /// <inheritdoc cref="RelayCommand(Action{object})"/>
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute) : this(execute, null) { }

        /// <summary>
        /// <inheritdoc cref="RelayCommand(Action{object}, Predicate{object})"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Check if command is executable
        /// </summary>
        /// <param name="parameter"></param>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => _execute((T)parameter);

        /// <summary>
        /// Eventhandles for changes in CanExecute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add    => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}

