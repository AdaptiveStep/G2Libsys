namespace G2Libsys.Dialogs
{
    /// <summary>
    /// Dialog window
    /// </summary>
    public interface IDialog
    {
        bool? DialogResult { get; set; }
        object DataContext { get; set; }

        bool? ShowDialog();
        void Close();
    }
}
