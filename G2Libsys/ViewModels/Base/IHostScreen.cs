namespace G2Libsys.ViewModels
{
    using G2Libsys.Library;

    public interface IHostScreen
    {
        public IViewModel CurrentViewModel { get; set; }

        public ISubViewModel SubViewModel { get; set; }

        public User CurrentUser { get; set; }
    }
}
