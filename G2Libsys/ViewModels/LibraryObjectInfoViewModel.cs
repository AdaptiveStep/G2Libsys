using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library.Models;
using G2Libsys.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{

    // Visa info om ett libraryobject med lånemöjligheter 
    public class LibraryObjectInfoViewModel : BaseViewModel, ISubViewModel
    {
        public ICommand CancelCommand => new RelayCommand(_ => NavService.HostScreen.SubViewModel = null);

        private ObservableCollection<LibraryObject> currentBooks;

        private ObservableCollection<Author> authors;

        public LibraryObjectInfoViewModel()
        {
           
        }

        

        public LibraryObjectInfoViewModel(LibraryObject libraryObject)
        {
            authors = new ObservableCollection<Author>();
            currentBooks = new ObservableCollection<LibraryObject>();
            authors.Add(new Author()
            {
                Name = "Bosse",
                Biography = @"874 är den finska landsbygden utarmad efter år av missväxt och svält. Aksel har sett många familjer försvinna eller gå under, och han vet att han har havet att tacka för att de ännu är vid liv. Hans älskade hustru och deras ­dotter.

De har ingenting annat än fisket att leva av.­ ­Drömmen om ett drägligare liv åt familjen har fått honom att blicka långt bortom horisonten.Mot ­Sverige.

Aksel står inför ett livsavgörande val.Ett oåter­kalleligt beslut måste fattas.Ett som kommer att sätta hans envishet,
                hustruns styrka och deras kärlek på prov.

Han ser bara en möjlighet.Att ensam ge sig ut över havet och lämna de han älskar bakom sig.

Nytt land baserar sig på en sann historia.n",
                ID = 1
            }); 
            
            authors.Add(new Author()
            {
                Name = "Caroline Arbo",
                Biography = @"aroline Albo är född 1984. Hon växte upp i Lindome, Göteborg, med sina föräldrar och sin tvillingbror. Caroline har bland annat bott i USA och Australien, rest jorden runt samt under flera år jobbat på en privatägd lyxyacht där hon haft en av världens rikaste människor som arbetsgivare och flera kontinenter som arbetsplats. 

Caroline har en examen inom Hospitality management och är diplomerad samtalsterapeut och energiterapeut på IRIZ Akademin i Göteborg. Hennes önskan är att öka medvetenheten kring psykisk och fysisk ohälsa och hjälpa andra människor till bättre psykisk hälsa och välmående.    ",
                ID = 2
            });
            var x = libraryObject;
            currentBooks.Add(libraryObject);
        }


        public ObservableCollection<Author> AuthorObjects
        {
            get => authors;
            set
            {
                authors = value;
                OnPropertyChanged(nameof(AuthorObjects));
            }
        }
        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => currentBooks;
            set
            {
                currentBooks = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }

    }
}
