using Test_Kiosk_MVVM.Models;
using Test_Kiosk_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Kiosk_MVVM.DB;

namespace Test_Kiosk_MVVM.MySQL
{
  public  class DataDownloader
  {
        private string dbHost;
        private string dbPort;
        private string dbName;
        private string dbUserName;
        private string dbPassword;


        public DataDownloader()
        {
            dbHost = ConfigurationManager.AppSettings.Get("DataBaseHost");
            dbPort = ConfigurationManager.AppSettings.Get("DataBasePort");
            dbName = ConfigurationManager.AppSettings.Get("DataBaseName");
            dbUserName = ConfigurationManager.AppSettings.Get("DataBaseUserName");
            dbPassword = ConfigurationManager.AppSettings.Get("DataBasePassword");
        }

        public List<Product> DownloadProductsFromDB()
        {
            List<Product> products = new List<Product>();
            using (ApplicationContext db = new()) 
            {
                foreach (DataUploader.Data.Product product in db.Products) 
                {
                    products.Add(new Product(product.Id,product.Name,product.Price,product.CategoryId,product.ImagePath));
                  //  ApplicationViewModel.logger.Info($"Добавлен продукт с id = {product.Id}, name = {product.Name}");
                }
            }
            return products;
        }
        public List<Category> DownloadCategoryFromDB()
        {
            List<Category> categories = new List<Category>();

            using (ApplicationContext db = new())
            {
                foreach (DataUploader.Data.Category category in db.Categories)
                {
                    categories.Add(new Category(category.Id,category.Name,category.ImagePath));
                    ApplicationViewModel.logger.Info($"Добавлена категория с id = {category.Id}, name = {category.Name}");
                }
            }
            return categories;
        }
    }
}
