using G2Libsys.Commands;
using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class ReasonDialogViewModel : BaseDialogViewModel<string>
    {

        private string reason;
        public string Reason
        {
            get => reason;
            set
            {
                reason = value;
                OnPropertyChanged(nameof(Reason));
            }
        }
        //private Card card;
        //public Card Card
        //{
        //    get => card;
        //    set
        //    {
        //        card = value;
        //        OnPropertyChanged(nameof(Card));
        //    }
        //}

        public ICommand SaveCommand => new RelayCommand(Result);
        private void Result(object param = null)
        {


            base.DialogResult = Reason;
            base.OKCommand.Execute(param);
        }
        public ReasonDialogViewModel(string Reason,  string title = null)
            : base(title)
        {
            this.Reason = Reason;

        }
    }
}

