using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUploader.Data
{
    public class Product
    {
        #region Public properties
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get;set;
        }
        public float Price
        {
            get;set;
        }
        public int CategoryId
        {
            get;set;
        }
        public string ImagePath 
        {
            get;set;
        }

        #endregion


        public Product( string name, float price, int categoryId, string imagePath)
        {      
            this.Name = name;
            this.Price = price;
            this.CategoryId = categoryId;
            this.ImagePath = imagePath;
        }
    }
}
