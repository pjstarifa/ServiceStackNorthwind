using System;
using System.Configuration;
using System.Web;
using Funq;
using Northwind.ServiceInterface;
using ServiceStack.Api.Swagger;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.MiniProfiler;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;

namespace Northwind
{
    /// <summary>
    ///     Create your ServiceStack web service application with a singleton AppHost.
    /// </summary>
    public class AppHost : AppHostBase
    {
        /// <summary>
        ///     Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost()
            : base("Northwind Web Services", typeof (CustomersService).Assembly)
        {
            
        }

        /// <summary>
        ///     Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
            container.Register<IDbConnectionFactory>(c =>
                                                     new OrmLiteConnectionFactory(connectionString,
                                                                                  SqlServerDialect.Provider));

            //Using an in-memory cache
            container.Register<ICacheClient>(new MemoryCacheClient());
            
            // Add the Swagger plugin 
            Plugins.Add(new SwaggerFeature());

            VCardFormat.Register(this);



            /*
            // Configure authentication
            Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] {
            new BasicAuthProvider()
        }));

            container.Register<ICacheClient>(new MemoryCacheClient());
            var userRep = new InMemoryAuthRepository();
            container.Register<IUserAuthRepository>(userRep);
            //Add a user for testing purposes
            string hash;
            string salt;
            string Password = "123";
            new SaltedHash().GetHashAndSaltString(Password, out hash, out salt);
            userRep.CreateUserAuth(new UserAuth
            {
                Id = 1,
                DisplayName = "DisplayName",
                Email = "as@if.com",
                UserName = "",
                FirstName = "FirstName",
                LastName = "LastName",
                PasswordHash = hash,
                Salt = salt,
            }, Password);
            // End configure authentication
           */
        }
    }

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AppHost()).Init();
        }
    }
}