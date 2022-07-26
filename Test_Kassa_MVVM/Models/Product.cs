using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Configuration;
using System.IO;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Test_Kiosk_MVVM.ViewModels;

namespace Test_Kiosk_MVVM.Models
{
  public  class Product : INotifyPropertyChanged
    {
        #region Private properties
        private int id;
        private string name;
        private float price;
        private int categoryId;
        [JsonIgnore]
        private Image image;
        #endregion

        #region Public properties
        public int Id
        {
            get { return id; }
            set
            { 
                id = value; 
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            { 
                name = value; 
                OnPropertyChanged("Name");
            }
        }
        public float Price
        {
            get { return price; }
            set 
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        public int CategoryId
        {
            get { return categoryId; }
            set 
            {
                categoryId = value; 
                OnPropertyChanged("CategoryId");
            }
        }
        [JsonIgnore]
        public Image Image
        {
            get 
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged("ImagePath");
            }
        }
        #endregion

        public Product(int id, string name, float price, int categoryId)
        {
            ApplicationViewModel.logger.Info($"Начинаем инициализацию продукта c дефолтной картинкой: Id {id},Название {name},Цена {price},Id категории {categoryId} ");
            try
            {
                this.id = id;
                this.name = name;
                this.price = price;
                this.categoryId = categoryId;
            }
            catch(Exception e)
            {
                ApplicationViewModel.logger.Error("Не верный формат информации о продукте \n" + e.Message);
            }
            try
            {
                var bitmapImg = new BitmapImage();

                bitmapImg.BeginInit();
                bitmapImg.UriSource = new Uri(ConfigurationManager.AppSettings.Get("DefaultImagePath"), UriKind.Absolute);
                bitmapImg.EndInit();

                Image = new Image();
                Image.Source = bitmapImg;
            }
            catch
            {
                ApplicationViewModel.logger.Error("По указанному в конфиг-файле пути нет картинки default.png.");
                throw new Exception("По указанному в конфиг-файле пути нет картинки default.png. Добавьте картинку или измените путь.");
            }
        }
        public Product(int id, string name, float price, int categoryId, string uri)
        {
            try
            {
                ApplicationViewModel.logger.Info($"Начинаем инициализацию продукта с индивидуальной картинкой:" +
                    $" Id {id},Название {name},Цена {price},Id категории {categoryId}, Путь к картинке {uri}");
                this.id = id;
                this.name = name;
                this.price = price;
                this.categoryId = categoryId;
            }
            catch (Exception e)
            {
                ApplicationViewModel.logger.Error("Не верный формат информации о продукте \n" + e.Message);
            }
            
            try
            {
                var bitmapImg = new BitmapImage();
                bitmapImg.BeginInit();
                bitmapImg.UriSource = new Uri(uri, UriKind.Absolute);
                bitmapImg.EndInit();

                Image = new Image();
                Image.Source = bitmapImg;
            }
            catch
            {
                try
                {
                    ApplicationViewModel.logger.Error("По указанному пути нет картинки. Ставим default.png");
                    var bitmapImg = new BitmapImage();
                    bitmapImg.BeginInit();
                    bitmapImg.UriSource = new Uri(ConfigurationManager.AppSettings.Get("DefaultImagePath"), UriKind.Absolute);
                    bitmapImg.EndInit();

                    Image = new Image();
                    Image.Source = bitmapImg;
                }
                catch 
                {
                    ApplicationViewModel.logger.Error("По указанному в конфиг-файле пути нет картинки default.png.");
                    throw new Exception("По указанному в конфиг-файле пути нет картинки default.png. Добавьте картинку или измените путь.");
                }
               
            }           
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
