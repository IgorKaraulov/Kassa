using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace PaginationControl.Models
{
   

    public class PaginationModel : INotifyPropertyChanged
    {

        private int currentPage;
        private int productsPerPage;
        private int totalPages;
        private int totalProducts;
        private int[] prodPerPageVariants;


        public int CurrentPage
        {
            get 
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }

        }
        public int ProductsPerPage
        {
            get
            {
                return productsPerPage;
            }
            set
            {
                productsPerPage = value;
                OnPropertyChanged("ProductsPerPage");
                TotalPages = CalculateTotalPages(TotalProducts,value);
            }

        }
        public int TotalPages
        {
            get
            {
                return totalPages;
            }
            set
            {
                totalPages = value;
                OnPropertyChanged("TotalPages");
            }

        }
        public int TotalProducts
        {
            get
            {
                return totalProducts;
            }
            set
            {
                totalProducts = value;
                OnPropertyChanged("TotalProducts");
            }

        }

        public int[] ProdPerPageVariants
        {
            get
            {
                return prodPerPageVariants;
            }
            private set
            {
                prodPerPageVariants = value;
            }

        }

        public ObservableCollection<Page> Pages { get; set; }


        public  PaginationModel(int productsCount, int prodPerPage)
        {
            CurrentPage = 1;
            
            TotalProducts = productsCount;
            ProductsPerPage = prodPerPage;
            TotalPages = CalculateTotalPages(productsCount,ProductsPerPage);

            Pages = new ObservableCollection<Page>();
            for (int i = 0; i < totalPages; i++)
            {
                Pages.Add(new Page(i+1));
            }
            prodPerPageVariants = new int[] { 1, 5, 10, 15 };

            PropertyChanged += OnPropertyTotalPagesChanged;
        }



        public PaginationModel(int productsCount)
        {
            CurrentPage = 1;

            TotalProducts = productsCount;
            ProductsPerPage = 5;
            TotalPages = CalculateTotalPages(productsCount, ProductsPerPage);

            Pages = new ObservableCollection<Page>();
            for (int i = 0; i < totalPages; i++)
            {
                Pages.Add(new Page(i + 1));
            }
            prodPerPageVariants = new int[] { 1, 5, 10, 15 };
            PropertyChanged += OnPropertyTotalPagesChanged;

        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private int CalculateTotalPages(int productsCount, int productsPerPage) 
        {

            int checker = 0;
            int totalPages = 0;
            Math.DivRem(productsCount,productsPerPage,out checker);
            totalPages = productsCount / productsPerPage;
            if (checker != 0) { totalPages++; }

            return totalPages;
        }

        private void OnPropertyTotalPagesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalPages")
            {
                Pages.Clear();
                for (int i = 0; i < totalPages; i++)
                {
                    Pages.Add(new Page(i + 1));
                }
            }
           
        }
    }
}
