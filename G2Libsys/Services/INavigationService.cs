namespace G2Libsys.Services
{
    using G2Libsys.ViewModels;
    using System.Collections.Generic;

    public interface INavigationService
    {
        IHostScreen HostScreen { get; }

        IList<IViewModel> ViewModels { get; }
    }
}
