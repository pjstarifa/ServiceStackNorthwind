using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace ServiceStack.Northwind.Tests
{
    [TestClass]
    public class ModelTest
    {

        private OrmLiteConnectionFactory _factory;
  

        [TestInitialize]
        public void Initialize()
        {
            var connStr = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
            OrmLiteConfig.DialectProvider = SqlServerOrmLiteDialectProvider.Instance;
            _factory = new OrmLiteConnectionFactory(connStr, true);
        }

        [TestMethod]
        public void EnsureModelsAreCorrect()
        {          
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                Assert.IsInstanceOfType(db.QuerySingle<Category>(""), typeof(Category));
                Assert.IsInstanceOfType(db.QuerySingle<Customer>(""), typeof(Customer));
                Assert.IsInstanceOfType(db.QuerySingle<Employee>(""), typeof(Employee));
                Assert.IsInstanceOfType(db.QuerySingle<EmployeeTerritory>(""), typeof(EmployeeTerritory));
                Assert.IsInstanceOfType(db.QuerySingle<OrderDetail>(""), typeof(OrderDetail));
                Assert.IsInstanceOfType(db.QuerySingle<Order>(""), typeof(Order));
                Assert.IsInstanceOfType(db.QuerySingle<Product>(""), typeof(Product));
                Assert.IsInstanceOfType(db.QuerySingle<Region>(""), typeof(Region));
                Assert.IsInstanceOfType(db.QuerySingle<Shipper>(""), typeof(Shipper));
                Assert.IsInstanceOfType(db.QuerySingle<Supplier>(""), typeof(Supplier));
                Assert.IsInstanceOfType(db.QuerySingle<Territory>(""), typeof(Territory));
            }
        }

        [TestMethod]
        public void EnsureServicesAreCorrect()
        {

        }
    }
}