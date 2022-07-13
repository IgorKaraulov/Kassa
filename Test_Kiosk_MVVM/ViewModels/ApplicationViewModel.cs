using Test_Kiosk_MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Windows.Input;
using Test_Kiosk_MVVM;
using Test_Kiosk_MVVM.NetworkInteraction;
using Newtonsoft.Json;
using System.Windows;
using PaginationControl.ViewModel;
using PaginationControl.Models;
using NLog;

namespace Test_Kiosk_MVVM.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public CategoryModel CategoryModel { get; set; }
        public OrderModel OrderModel { get; set; }
        private Category selectedCategory;
        private Product selectedItem;
        private OrderMessager messager;
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private PaginationViewModel paginationViewModel;
        public PaginationViewModel PaginationViewModel
        {
            get
            {
                return paginationViewModel;
            }

            set
            {
                paginationViewModel = value;
                OnPropertyChanged("PaginationViewModel");
            }
        }
        public Action onButtonDownClicked;
        public Action onButtonUpClicked;
        
        public Product SelectedItem
        {
            get { return selectedItem; }
            set
            { 
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        private ScrollViewer categoryPanel;

        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set 
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        public ApplicationViewModel()
        {            
            CategoryModel = new CategoryModel();
            OrderModel = new OrderModel();

            OrderModel.Products = new ObservableCollection<Product>();
            
            CategoryModel.ProductsView = new ObservableCollection<Product>();

            var productsFirstCategory = CategoryModel.GetProductsWithCategoryId(1);

            for (int i = 0; i < productsFirstCategory.Count; i++)
            {
                CategoryModel.ProductsView.Add(productsFirstCategory[i]);
            }
 
            //При внедрении пагинатора я использую именно ProductsView.Count, а не общее кол-во продуктов, потому что есть ещё
            //сортировка по категориям. И ProductsView.Count как раз таки выдаёт мне количество отсортированных элементов            
            PaginationModel pag = new PaginationModel(CategoryModel.ProductsView.Count);
            
            PaginationViewModel = new PaginationViewModel(pag);
            PaginationViewModel.PaginationModel.PropertyChanged += CurrentPagePropertyChanged;

            UpdateProductList(1);      
        }

        #region Commands
        public ICommand SendMessage
        {
            get
            {
                return new RelayCommand(param => { SendOrder(); });
            }
        }
        public ICommand UpdateItemList
        {
            get
            {
                return new RelayCommand(param => { UpdateProductList(param); });
            }
        }
        public ICommand AddProductToOrder
        {
            get
            {
                return new RelayCommand(param => { AddProduct((int)param); });
            }
        }
        public ICommand RemoveProductFromOrder
        {
            get
            {
                return new RelayCommand(param => { DeleteProduct((int)param); });
            }
        }
        public ICommand CategoryUpdate
        {
            get
            {
                return new RelayCommand(param => { UpdateProductsWithCategory((int)param); });
            }
        }
        public ICommand CategoryButtonDown
        {
            get
            {
                return new RelayCommand(param => { CategoryButtonDownClick(); });
            }
        }
        public ICommand CategoryButtonUp
        {
            get
            {
                return new RelayCommand(param => { CategoryButtonUpClick(); });
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #region CommandMethods
        private void UpdateProductsWithCategory(int id)
        {
            try
            {
                logger.Info("Запускаем метод UpdateCategory");
                CategoryModel.ProductsView.Clear();
                var products = CategoryModel.GetProductsWithCategoryId(id);

                for (int i = 0; i < products.Count; i++)
                {
                    CategoryModel.ProductsView.Add(products[i]);
                }

                //Не знаю на сколько это страшно с точки зрения расхода памяти... Но пока так.
                paginationViewModel.PaginationModel.Pages.Clear();
                PaginationModel pag = new PaginationModel(CategoryModel.ProductsView.Count, paginationViewModel.PaginationModel.ProductsPerPage);
                PaginationViewModel = new PaginationViewModel(pag);
                PaginationViewModel.PaginationModel.PropertyChanged += CurrentPagePropertyChanged;
                UpdateProductList(null);
            }
            catch (Exception e)
            {
                logger.Error("Ошибка. Метод UpdateCategory \n" + e.Message);
            }
        }

        //Этот метод будет привязан к каждой кнопке и будет передавать свой номер
        private void UpdateProductList(object param)
        {
            try
            {
                logger.Info("Пытаемся обновить лист продуктов");
                CategoryModel.ProductsViewWithPaginator.Clear();
                List<Product> products = new List<Product>();

                int startCount = (PaginationViewModel.PaginationModel.CurrentPage - 1) * (PaginationViewModel.PaginationModel.ProductsPerPage);
                int endingCount = startCount + paginationViewModel.PaginationModel.ProductsPerPage;

                products = CategoryModel.ProductsView.Skip(startCount).Take(PaginationViewModel.PaginationModel.ProductsPerPage).ToList();
                for (int i = 0; i < products.Count; i++)
                {
                    CategoryModel.ProductsViewWithPaginator.Add(products[i]);
                }
            }
            catch (Exception e)
            {
                logger.Error("Ошибка на стадии обновления листа продуктов. Метод UpdateProductList \n" + e.Message);
            }
        }
        private void CurrentPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                UpdateProductList(null);
            }
        }
        private void AddProduct(int idProduct)
        {
            try
            {
                logger.Info($"Пытаемся добавить продукт в заказ. Продукт  с id {idProduct}");

                var product = CategoryModel.GetProductWithId(idProduct);
               
                OrderModel.Products.Add(product);
                OrderModel.Sum += product.Price;
                logger.Info($"Продукт {product.Name} с id {product.Id} успешно добавлен");
            }
            catch (Exception e)
            {
                logger.Error("Ошибка добавления продукта в заказ \n" + e.Message);
            }
        }
        private void DeleteProduct(int idProduct)
        {
            try
            {
                logger.Info($"Пытаемся удалить продукт с id {idProduct} из заказа");
                var product = OrderModel.GetProductWithId(idProduct);
               
                OrderModel.Products.Remove(product);
                OrderModel.Sum -= product.Price;
            }
            catch (Exception e)
            {
                logger.Error("Ошибка при удалении продукта из заказа \n" + e.Message);
            }        
        }
        private  void SendOrder()
        {
            messager = new OrderMessager();
            
            var serializedMessage = JsonConvert.SerializeObject(OrderModel);
            //Метод SendMessage возвращает так же ответ сервера о статусе доставки.
            try
            {
                logger.Info("Пытаемся отправить заказ серверу");
                var response = messager.SendMessage(serializedMessage);
                MessageBox.Show(response);
            }
            catch (Exception e)
            {
                logger.Error("Ошибка при отправке заказа серверу" + e.Message);
            }
        }

        private void CategoryButtonDownClick()
        {
            if (categoryPanel.VerticalOffset == categoryPanel.ScrollableHeight && CategoryModel.Categories.Count > 0)
            {
                var categoryScroll = CategoryModel.Categories.First(); 

                CategoryModel.Categories.Remove(CategoryModel.Categories.First());
                CategoryModel.Categories.Add(categoryScroll);
                //Вместо 100 везде должна быть ширина кнопки.
                categoryPanel.ScrollToVerticalOffset(categoryPanel.VerticalOffset - 100);
            }
            onButtonDownClicked.Invoke();
        }
        private void CategoryButtonUpClick()
        {
            if (categoryPanel.VerticalOffset == 0 && CategoryModel.Categories.Count>0)
            {
                var categoryScroll = CategoryModel.Categories.Last();

                CategoryModel.Categories.Remove(CategoryModel.Categories.Last());
                CategoryModel.Categories.Insert(0,categoryScroll);
                categoryPanel.ScrollToVerticalOffset(categoryPanel.VerticalOffset + 100);
            }
            onButtonUpClicked.Invoke();
        }
        #endregion

        public void AddScroll(ScrollViewer SV)
        {
           categoryPanel = SV;
        }
    }
}
