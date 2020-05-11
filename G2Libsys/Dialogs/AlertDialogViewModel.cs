namespace G2Libsys.Dialogs
{
    public class AlertDialogViewModel : BaseDialogViewModel<bool?>
    {
        public AlertDialogViewModel(string title = null, string msg = null) 
            : base(title, msg) { }
    }
}
