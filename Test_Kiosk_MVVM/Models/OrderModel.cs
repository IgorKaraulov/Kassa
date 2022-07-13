using Test_Kiosk_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test_Kiosk_MVVM.Models
{
   public class OrderModel: INotifyPropertyChanged
   {
        //Сумма в заказе
        private float sum;


        //Список продуктов отображаемых в заказе (View)
        public ObservableCollection<Product> Products { get; set; }


        public float Sum 
        {
            get { return sum; }
            set
            {
                sum = value;
                OnPropertyChanged("Sum");
            }
        }

        public Product GetProductWithId(int id)
        {
            for (int i = 0; i < Products.Count; i++) 
            {
                if (Products[i].Id == id)
                {
                    return Products[i];
                    
                }
            }
            ApplicationViewModel.logger.Error("Неверный id продукта при поиске в списке");
            throw new Exception("Товара с таким id нет в заказе");
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
