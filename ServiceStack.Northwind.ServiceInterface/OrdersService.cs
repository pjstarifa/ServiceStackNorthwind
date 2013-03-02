using System.Collections.Generic;
using System.Linq;
using Northwind.ServiceModel.Types;
using ServiceStack.Common.Extensions;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Northwind.ServiceInterface
{
    [Route("/orders")]
    [Route("/orders/page/{Page}")]
    [Route("/customers/{CustomerId}/orders")]
    public class Orders
    {
        public int? Page { get; set; }
        public string CustomerId { get; set; }
    }

    public class OrdersResponse
    {
        public OrdersResponse()
        {
            ResponseStatus = new ResponseStatus();
            Results = new List<CustomerOrder>();
        }

        public List<CustomerOrder> Results { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }


    public class OrdersService : Service
    {
        private const int PageCount = 20;

        public OrdersResponse Get(Orders request)
        {
            //TODO: Figure out a paging strategy for SQL Server
            List<Order> orders = request.CustomerId.IsNullOrEmpty()
                                     //? Db.Select<Order>("ORDER BY OrderDate DESC LIMIT {0}, {1}", (request.Page.GetValueOrDefault(1) - 1) * PageCount, PageCount)
                                     ? Db.Select<Order>("ORDER BY OrderDate DESC",
                                                        (request.Page.GetValueOrDefault(1) - 1)*PageCount, PageCount)
                                     : Db.Select<Order>("CustomerId = {0}", request.CustomerId);

            if (orders.Count == 0)
            {
                return new OrdersResponse();
            }

            List<OrderDetail> orderDetails = Db.Select<OrderDetail>(
                "OrderId IN ({0})", new SqlInValues(orders.ConvertAll(x => x.Id)));

            ILookup<int, OrderDetail> orderDetailsLookup = orderDetails.ToLookup(o => o.OrderId);

            List<CustomerOrder> customerOrders = orders.ConvertAll(o =>
                                                                   new CustomerOrder
                                                                       {
                                                                           Order = o,
                                                                           OrderDetails =
                                                                               orderDetailsLookup[o.Id].ToList()
                                                                       });

            return new OrdersResponse {Results = customerOrders};
        }
    }
}