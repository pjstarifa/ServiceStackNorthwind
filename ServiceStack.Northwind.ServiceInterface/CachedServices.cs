using ServiceStack.Common;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Northwind.ServiceInterface
{
    [Route("/cached/customers")]
    public class CachedCustomers
    {
    }

    [Route("/cached/customers/{Id}")]
    public class CachedCustomerDetails
    {
        public string Id { get; set; }
    }

    [Route("/cached/orders")]
    [Route("/cached/orders/page/{Page}")]
    [Route("/cached/customers/{CustomerId}/orders")]
    public class CachedOrders
    {
        public int? Page { get; set; }
        public string CustomerId { get; set; }
    }

    /// <summary>
    ///     Create your ServiceStack RESTful web service implementation.
    /// </summary>
    public class CachedCustomersService : Service
    {
//        public object Get(CachedCustomers request)
//        {
//            //Manually create the Unified Resource Name "urn:customers".
//            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, "urn:customers", () =>
//                {
//                    //Resolve the service in order to get the customers.
//                    using (var service = ResolveService<CustomersService>())
//                        return service.Any(new AllCustomers());
//                });
//        }

        public object Get(CachedCustomerDetails request)
        {
            //Create the Unified Resource Name "urn:customerdetails:{id}".
            string cacheKey = UrnId.Create<CustomerDetails>(request.Id);
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, cacheKey, () =>
                {
                    using (var service = ResolveService<CustomerDetailsService>())
                    {
                        return service.Get(new CustomerDetails {Id = request.Id});
                    }
                });
        }

        public object Get(CachedOrders request)
        {
            string cacheKey = UrnId.Create<Orders>(request.CustomerId ?? "all",
                                                   request.Page.GetValueOrDefault(0).ToString());
            return base.RequestContext.ToOptimizedResultUsingCache(Cache, cacheKey, () =>
                {
                    using (var service = ResolveService<OrdersService>())
                    {
                        return service.Get(new Orders {CustomerId = request.CustomerId, Page = request.Page});
                    }
                });
        }
    }
}