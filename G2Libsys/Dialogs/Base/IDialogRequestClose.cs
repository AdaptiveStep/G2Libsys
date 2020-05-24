namespace G2Libsys.Dialogs
{
    using G2Libsys.Events;
    using System;

    /// <summary>
    /// Interface for implementation of CloseDialog event
    /// </summary>
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
