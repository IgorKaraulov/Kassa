using System;
using System.Collections.Generic;
using System.Text;

namespace TCP_IP_Server
{
   public class Order
   {
        private List<Product> products;
        private float orderSum;

        public List<Product> Products
        {
            get 
            {
                return products;
            }
            set
            {
                products = value;
            }      
        }
        public float OrderSum 
        {
            get
            {
                return orderSum;
            }
            set
            {
                orderSum = value;
            }
        }

        public Order(List<Product> products, float sum)
        {
            this.products = products;
            this.orderSum = sum;
        }
   }
}
