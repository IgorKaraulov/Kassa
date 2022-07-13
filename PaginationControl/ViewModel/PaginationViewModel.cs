using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using PaginationControl.Models;

namespace PaginationControl.ViewModel
{
    public class PaginationViewModel : INotifyPropertyChanged
    {
        #region Properties
        private PaginationModel paginationModel;
        public PaginationModel PaginationModel
        {
            get
            {
                return paginationModel;
            }
            set
            {
                paginationModel = value;
                OnPropertyChanged("PaginationModel");
            }
        }

        #endregion
        
        

        #region Commands

        public ICommand ChangePage
        {
            get
            {
                return new RelayCommand(param => { PageChange((int)param); });
            } 
        }

       
        #endregion

        #region CommandMethods
        private void PageChange(int num)
        {
            paginationModel.CurrentPage = num;
        }

        #endregion

     public PaginationViewModel(PaginationModel pm)
     {
            PaginationModel = pm;
            PaginationModel.PropertyChanged += OnItemsPerPageCountChanged;
     }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void OnItemsPerPageCountChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProductsPerPage")
            {
                PageChange(1);
            }
        }

    }
}
