using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Test_Kiosk_MVVM.ViewModels;
using System.Configuration;
using Test_Kiosk_MVVM.MySQL;

namespace Test_Kiosk_MVVM.Models
{
   public class CategoryModel: INotifyPropertyChanged
    {
        public ObservableCollection<Category> Categories { get; set; }
        //Список продуктов, с учётом выбранной категории
        public ObservableCollection<Product> ProductsView { get; set; }

        //Конечный список продуктов, отображаемый на View с учетом категории и пагинатора
        public ObservableCollection<Product> ProductsViewWithPaginator { get; set; }

        //Список всех продуктов в системе
        private List<Product> products;

        public CategoryModel()
        {
            ProductsViewWithPaginator = new ObservableCollection<Product>();     

            DataDownloader downloader = new DataDownloader();
            products = downloader.DownloadProductsFromDB();
            Categories = new ObservableCollection<Category>(downloader.DownloadCategoryFromDB());
        }
        public List<Product> GetProductsWithCategoryId(int id)
        {
            List<Product> returnProducts = new List<Product>();

            for (int i = 0; i < products.Count; i++)
            {
                if (id == products[i].CategoryId)
                {
                    returnProducts.Add(products[i]);
                }
            }
            return returnProducts;
        }
        public Product GetProductWithId(int id)
        {    
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Id == id)
                {
                    return products[i];
                }
            }
            throw new Exception("Нет продукта с таким id");
        }
        public Category GetCategoryWithId(int id)
        { 
            foreach (Category cat in Categories)
            {
                if (cat.Id == id)
                {
                    return cat;
                }              
            }
            ApplicationViewModel.logger.Error($"Среди всех категорий нет ни одной с id = {id}");

            throw new Exception($"Среди всех категорий нет ни одной с id = {id}");    
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
   }
}
