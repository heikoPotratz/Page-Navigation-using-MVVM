﻿using System.Windows.Controls;
using System.Windows.Input;
using Page_Navigation_App.Utiel;

namespace Page_Navigation_App.ViewModel
{
    internal class NavigationVM : ViewModelBase
    {
        private object _currentView;

        private Grid _theGrid;

        public NavigationVM()
        {
            HamburgerMenuCommand = new RelayCommand(HamburgerMenu);
            HomeCommand = new RelayCommand(Home);
            CustomersCommand = new RelayCommand(Customer);
            ProductsCommand = new RelayCommand(Product);
            OrdersCommand = new RelayCommand(Order);
            TransactionsCommand = new RelayCommand(Transaction);
            TrebleClefCommand = new RelayCommand(TrebleClef);
            ShipmentsCommand = new RelayCommand(Shipment);
            SettingsCommand = new RelayCommand(Setting);

            // Startup Page
            CurrentView = new HomeVM();
        }

        /// <summary>
        /// Gets or sets the current view object.
        /// </summary>
        public object CurrentView
        {
            /*            get
                        {
                            return _currentView;
                        }
                        set
                        {
                            _currentView = value; OnPropertyChanged();
                        }*/
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged();

                // Aktualisieren der TheClefGrid-Eigenschaft
                if (_currentView is TrebleClefVM trebleClefVM)
                {
                    // wird ausgeführt für TrebleClevVM
                    TheGrid = trebleClefVM.TheClefGrid;
                }
                else
                {
                    TheGrid = null;
                }
            }
        }

        public ICommand CustomersCommand
        {
            get; set;
        }

        public ICommand HamburgerMenuCommand
        {
            get; set;
        }

        public ICommand HomeCommand
        {
            get; set;
        }

        public ICommand OrdersCommand
        {
            get; set;
        }

        public ICommand ProductsCommand
        {
            get; set;
        }

        public ICommand SettingsCommand
        {
            get; set;
        }

        public ICommand ShipmentsCommand
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the grid object for the Treble Clef view.
        /// </summary>
        public Grid TheGrid
        {
            get
            {
                return _theGrid;
            }
            set
            {
                _theGrid = value;
                OnPropertyChanged();
            }
        }

        public ICommand TransactionsCommand
        {
            get; set;
        }

        public ICommand TrebleClefCommand

        {
            get; set;
        }

        private void Customer(object obj) => CurrentView = new CustomerVM();

        // Change to only switch width from default to narrow and bach again ...
        private void HamburgerMenu(object obj) => CurrentView = new HamburgerMenuVM();

        private void Home(object obj) => CurrentView = new HomeVM();

        private void Order(object obj) => CurrentView = new OrderVM();

        private void Product(object obj) => CurrentView = new ProductVM();

        private void Setting(object obj) => CurrentView = new SettingVM();

        private void Shipment(object obj) => CurrentView = new ShipmentVM();

        private void Transaction(object obj) => CurrentView = new TransactionVM();

        private void TrebleClef(object obj) => CurrentView = new TrebleClefVM();
    }
}