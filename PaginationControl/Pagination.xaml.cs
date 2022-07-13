using PaginationControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaginationControl
{
    /// <summary>
    /// Логика взаимодействия для Pagination.xaml
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
            Comb.SelectedItem = 5;
        }

        private void Comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = Convert.ToInt32((sender as ComboBox).SelectedItem);
            (DataContext as PaginationViewModel).PaginationModel.ProductsPerPage = selected;
        }
    }
}
