namespace EntityFrameworkWpfApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Caliburn.Micro;
    using Domain.DataAccess;
    using Domain.Models;

    public class CustomerViewModel : PropertyChangedBase
    {
        private Func<IUnitOfWork> _uowFactory;
        private Customer _selectedCustomer = null;
        private string _firstName;
        private string _lastName;

        public CustomerViewModel(Func<IUnitOfWork> uowFactory)
        {
            _uowFactory = uowFactory;

            LoadData();
        }

        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();

        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                if (value != _selectedCustomer)
                {
                    _selectedCustomer = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public void LoadData()
        {
            Customers.Clear();

            using (var uow = _uowFactory())
            {
                var customers = uow.Customers.GetAllAsync()
                                             .Result
                                             .OrderBy(c => c.LastName + c.FirstName);

                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            }
        }

        public void AddNewCustomer()
        {
            if (!string.IsNullOrWhiteSpace(_firstName)
                && !string.IsNullOrWhiteSpace(_lastName))
            {
                using (var uow = _uowFactory())
                {
                    var result = uow.Customers.CreateAsync(new Customer { FirstName = _firstName, LastName = _lastName }).Result;
                    // TODO: check result
                }

                LoadData();
            }
        }

        public void DeleteCustomer()
        {
            if (_selectedCustomer != null)
            {
                using (var uow = _uowFactory())
                {
                    Customer customer = uow.Customers.GetAsync(_selectedCustomer.Id).Result;

                    if (uow.Customers.DeleteAsync(customer).Result)
                    {
                        _selectedCustomer = null;
                    }
                }
            }

            LoadData();
        }
    }
}
