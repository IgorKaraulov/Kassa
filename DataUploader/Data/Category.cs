using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUploader.Data
{
    public class Category
    {
        #region Public Properties
        public string Name { get; set; }
        public int Id { get; set; }
        public string ImagePath { get; set; }
        #endregion

        public Category(string name,  string imagePath)
        {
            this.Name = name;
            this.ImagePath = imagePath;
        }
    }
}
