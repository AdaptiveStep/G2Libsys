namespace G2Libsys.Events
{
    using System;

    /// <summary>
    /// Eventhandler for closing a dialog with dialogresult
    /// </summary>
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; }
    }
}
