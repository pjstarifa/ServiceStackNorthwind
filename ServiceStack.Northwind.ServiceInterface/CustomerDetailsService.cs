using System;
using System.Collections.Generic;
using System.Net;
using Northwind.ServiceModel.Types;
using ServiceStack.Common.Web;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Northwind.ServiceInterface
{
    [Route("/customers/{Id}")]
    public class CustomerDetails
    {
        public string Id { get; set; }
    }

    public class CustomerDetailsResponse : IHasResponseStatus
    {
        public CustomerDetailsResponse()
        {
            ResponseStatus = new ResponseStatus();
            CustomerOrders = new List<CustomerOrder>();
        }

        public Customer Customer { get; set; }
        public List<CustomerOrder> CustomerOrders { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class CustomerDetailsService : Service
    {
        public CustomerDetailsResponse Get(CustomerDetails request)
        {
            var customer = Db.IdOrDefault<Customer>(request.Id);
            if (customer == null)
                throw new HttpError(HttpStatusCode.NotFound,
                                    new ArgumentException("Customer does not exist: " + request.Id));

            using (var ordersService = base.ResolveService<OrdersService>())
            {
                OrdersResponse ordersResponse = ordersService.Get(new Orders {CustomerId = request.Id});

                return new CustomerDetailsResponse
                    {
                        Customer = customer,
                        CustomerOrders = ordersResponse.Results,
                    };
            }
        }
    }
}