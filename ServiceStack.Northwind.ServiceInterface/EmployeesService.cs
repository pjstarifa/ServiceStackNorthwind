using System.Collections.Generic;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Northwind.ServiceInterface
{
    [Route("/employees")]
    public class Employees
    {
    }

    public class EmployeesResponse
    {
        public EmployeesResponse()
        {
            ResponseStatus = new ResponseStatus();
            Employees = new List<Employee>();
        }

        public List<Employee> Employees { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    class EmployeesService: Service
    {
        public EmployeesResponse Get(Employees request)
        {
            return new EmployeesResponse { Employees = base.Db.Select<Employee>() };
        }
    }
}
