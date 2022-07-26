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
    public class Category : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private Image image;

        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
            SetDefaultImage();
        }

        public Category(int id, string name, string uri)
        {
            this.id = id;
            this.name = name;
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
                ApplicationViewModel.logger.Error("По указанному пути нет картинки. Ставим default.png");
                SetDefaultImage();
            }
        }

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
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetDefaultImage()
        {
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
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
