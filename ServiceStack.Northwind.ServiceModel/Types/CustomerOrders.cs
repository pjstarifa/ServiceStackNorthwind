using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Northwind.ServiceModel.Types;

namespace Northwind.ServiceModel.Types
{	
    public class CustomerOrder
    {
        public CustomerOrder()
        {
            this.OrderDetails = new List<OrderDetail>();
        }
                
        public Order Order { get; set; }		
        public List<OrderDetail> OrderDetails { get; set; }
    }
}