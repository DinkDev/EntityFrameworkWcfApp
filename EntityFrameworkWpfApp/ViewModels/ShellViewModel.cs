namespace EntityFrameworkWpfApp.ViewModels
{
    using Caliburn.Micro;

    public class ShellViewModel : Screen, IShell
    {
        public ShellViewModel(CustomerViewModel customer)
        {
            Customer = customer;
        }

        public CustomerViewModel Customer { get; }
    }
}
