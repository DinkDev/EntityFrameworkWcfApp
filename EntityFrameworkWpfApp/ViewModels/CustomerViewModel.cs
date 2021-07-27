namespace EntityFrameworkWpfApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Caliburn.Micro;
    using Domain.DataAccess;
    using Domain.Models;

    public class CustomerViewModel : PropertyChangedBase
    {
        private readonly Func<IUnitOfWork> _uowFactory;
        private CustomerViewItem _selectedCustomer = null;

        public CustomerViewModel(Func<IUnitOfWork> uowFactory)
        {
            _uowFactory = uowFactory;

            LoadData();
        }

        public ObservableCollection<CustomerViewItem> Customers { get; } = new ObservableCollection<CustomerViewItem>();

        public CustomerViewItem SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (value != _selectedCustomer)
                {
                    _selectedCustomer = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange(nameof(FirstName));
                    NotifyOfPropertyChange(nameof(LastName));
                }
            }
        }

        public string FirstName
        {
            get => SelectedCustomer?.FirstName ?? "NA";
            set
            {
                if (SelectedCustomer != null
                    && value != SelectedCustomer.FirstName)
                {
                    SelectedCustomer.FirstName = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public string LastName
        {
            get => SelectedCustomer?.LastName ?? "NA";
            set
            {
                if (SelectedCustomer != null
                    && value != SelectedCustomer.LastName)
                {
                    SelectedCustomer.LastName = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public void LoadData()
        {
            Customers.Clear();

            using (var uow = _uowFactory())
            {
                var customers = uow.Customers.GetAll()
                    .OrderBy(c => c.LastName + c.FirstName);

                foreach (var customer in customers)
                {
                    Customers.Add(new CustomerViewItem(customer));
                }
            }
        }

        public void AddNewCustomer()
        {
            var newCustomer = new CustomerViewItem(new Customer{ FirstName = "First", LastName = "Last"});
            Customers.Add(newCustomer);
            SelectedCustomer = newCustomer;

            //if (!string.IsNullOrWhiteSpace(_firstName)
            //    && !string.IsNullOrWhiteSpace(_lastName))
            //{
            //    using (var uow = _uowFactory())
            //    {
            //        var result = uow.Customers.Create(new Customer { FirstName = _firstName, LastName = _lastName });
            //        // TODO: check result
            //    }

            //    LoadData();
            //}
        }

        public void DeleteCustomer()
        {
            if (_selectedCustomer != null)
            {
                using (var uow = _uowFactory())
                {
                    Customer customer = uow.Customers.Get(_selectedCustomer.Wrapped.Id);

                    if (uow.Customers.Delete(customer))
                    {
                        _selectedCustomer = null;
                    }
                }
            }

            LoadData();
        }

        public void UpdateCustomer()
        {
            using (var uow = _uowFactory())
            {
                foreach (var currentCustomer in Customers.Select(w => w.Wrapped))
                {
                    var customer = uow.Customers.Get(currentCustomer.Id);

                    if (customer == null)
                    {
                        uow.Customers.Create(currentCustomer);
                    }
                    else
                    {
                        customer.FirstName = currentCustomer.FirstName;
                        customer.LastName = currentCustomer.LastName;

                        uow.Customers.Update(customer);
                    }
                }
            }

            LoadData();
        }

        public class CustomerViewItem : PropertyChangedBase
        {
            public CustomerViewItem(Customer wrapped)
            {
                Wrapped = wrapped;
            }

            public string FirstName
            {
                get => Wrapped.FirstName;
                set
                {
                    if (value != Wrapped.FirstName)
                    {
                        Wrapped.FirstName = value;
                        NotifyOfPropertyChange();
                    }
                }
            }

            public string LastName
            {
                get => Wrapped.LastName;
                set
                {
                    if (value != Wrapped.LastName)
                    {
                        Wrapped.LastName = value;
                        NotifyOfPropertyChange();
                    }
                }
            }

            public Customer Wrapped { get; set; }
        }
    }
}
