using System;
using System.Collections.Generic;
using System.Text;

namespace TCP_IP_Server
{
    public  class Product
    {
        #region Private properties
        private int id;
        private string name;
        private float price;
        private int categoryId;
        #endregion

        #region Public properties
        public int Id
        {
            get { return id; }
            set
            {
                id = value; 
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public float Price
        {
            get { return price; }
            set
            {
                price = value;
            }
        }
        public int CategoryId
        {
            get { return categoryId; }
            set
            {
                categoryId = value;
            }
        }
        #endregion
    }
}
