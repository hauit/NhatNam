using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Test.Models.DAO;
using WPF_Test.Models.Entity;

namespace WPF_Test.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private ObservableCollection<WebUrl> _webUrl;

        public ObservableCollection<WebUrl> WebUrl
        {
            get { return _webUrl; }
            set
            {
                if (_webUrl != value)
                {
                    _webUrl = value;
                }
            }
        }

        private WebUrl _selectedItem;
        public WebUrl SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }


        public ICommand SaveCommand { get; private set; }
        public ICommand EnterKeyPressedCommand { get; private set; }

        public ProductsViewModel()
        {
            LoadData(null);
            SaveCommand = new RelayCommand(SaveData);
            EnterKeyPressedCommand = new RelayCommand(EnterKeyPressed);
        }

        private void LoadData(object data)
        {
            Properties.Settings.Default.Server = "192.168.1.116";
            Properties.Settings.Default.User = "sa";
            Properties.Settings.Default.Password = "123456";
            Properties.Settings.Default.Database = "KhangMinh";
            var a = new WebUrlDAO();
            var b = a.GetAllData("select * from [222_WebUrl]");
            WebUrl = new ObservableCollection<WebUrl>(b);

            //OnPropertyChanged(nameof(Products));
        }

        private void EnterKeyPressed(object parameter)
        {
            // Xử lý khi người dùng nhấn phím Enter
            var selectedItem = parameter as WebUrl;
            if (selectedItem != null)
            {
                ValidateAndUpdate(selectedItem);
            }

            //OnPropertyChanged(nameof(WebUrl));
        }

        private void ValidateAndUpdate(WebUrl selectedItem)
        {
            string sql = $@"update [222_WebUrl] set Note = N'{selectedItem.Note}' where ID ={selectedItem.ID}";
            var a = new WebUrlDAO();
            var b = a.ExecuteQuery(sql);

            if(selectedItem.ID == 0)
            {
                WebUrl.Remove(selectedItem);
            }
        }

        private void SaveData(object data)
        {
            var selectedItem = data as Products;

            //OnPropertyChanged(nameof(Products));
        }
    }

    #region
    //public class TableViewModel<T> : ViewModelBase
    //{
    //    private ObservableCollection<T> _items;
    //    public ObservableCollection<T> Items
    //    {
    //        get { return _items; }
    //        set { SetProperty(ref _items, value); }
    //    }

    //    public ICommand LoadDataCommand { get; private set; }
    //    public ICommand ClearDataCommand { get; private set; }

    //    public TableViewModel()
    //    {
    //        Items = new ObservableCollection<T>();
    //        LoadDataCommand = new RelayCommand(LoadData);
    //        ClearDataCommand = new RelayCommand(ClearData);
    //    }

    //    private void LoadData(object parameter)
    //    {
    //        // Logic để tải dữ liệu từ bảng tương ứng (sử dụng Dependency Injection hoặc các service)
    //    }

    //    private void ClearData(object parameter)
    //    {
    //        Items.Clear();
    //    }
    //}
    #endregion
}
