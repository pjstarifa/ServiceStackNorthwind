using System.Collections.Generic;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Northwind.ServiceInterface
{


    #region CustomerService
    public class CustomerService: Service
    {
        public void Put(Customer request)
        {
            Db.Update<Customer>();
        }
    } 
    #endregion



    #region Customers Routes
    [Route("/customerSearch")]
    public class CustomerSearch : IReturn<List<Customer>>
    {
        public CustomerSearch(CustomerSearch request)
        {
            CompanyName = request.CompanyName;

        }
        public string CompanyName { get; set; }

    }

    [Route("/customers")]
    public class AllCustomers : IReturn<List<Customer>>
    {
    }

    #endregion
    #region Customers Service
    public class CustomersService : Service
    {
        public List<Customer> Any(CustomerSearch request)
        {
            var sql = string.Format("select * from Customers where CompanyName like '{0}%'", request.CompanyName);
            var customers= Db.Select<Customer>(sql);
            return customers;
        }

        public List<Customer> Get(AllCustomers request)
        {
            var customers= Db.Select<Customer>("select * from customers order by CompanyName ");
            return customers;
        }
//
//        public Customer Put(Customer request)
//        {
//           Db.Update<Customer>(request);
//            var customer = Db.Id<Customer>(request.Id);
//          return customer;
//        }
//
//        public Customer Post(Customer request)
//        {
//            Db.Insert<Customer>(request);
//            var id = Db.GetLastInsertId();
//            var customer = Db.Id<Customer>(id);
//            return customer;
//        }

    }
    
    #endregion
}