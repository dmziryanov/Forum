using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Core.Resource;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Ioc_Windsor;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using System.Threading;
using AdvSpareAuto.Controllers;
using BootStrap;
using BootStrap.Filters;
using CustomRoutes.Models;
using DAL;
using Inspinia_MVC5.Controllers;
using WebMatrix.WebData;

namespace Inspinia_MVC5
{





    class SimpleMembershipInitializer
    {
        public SimpleMembershipInitializer()
        {
            Database.SetInitializer<UsersContext>(null);

            try
            {
                using (var context = new UsersContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    }
                }

                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
            }
        }
    }

    public class MvcApplication : System.Web.HttpApplication, IContainerAccessor
    {
        private static IWindsorContainer _container;
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        public MvcApplication()
        {

        }

        protected void Application_Start()
        {
            // WebSecurity.InitializeDatabaseConnection("DefaultConnection", "System.Data.SQlClient", "UserId", "UserName", true);
            var filename = "assembly://Inspinia_MVC5/windsor.config.xml";
            IResource resource = new AssemblyResource(filename);
            _container = new WindsorContainer(new XmlInterpreter(resource));



            
           

            //add all of the controllers to the container
            //foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof(IController).IsAssignableFrom(x)))
            {
                
                   _container.Register(Component.For(typeof(HomeController)).LifestylePerWebRequest());
                    _container.Register(Component.For(typeof(ForumController)).LifestylePerWebRequest());
                _container.Register(Component.For(typeof(AccountController)).LifestylePerWebRequest());
                _container.Register(Component.For(typeof(AppViewsController)).LifestylePerWebRequest());
                _container.Register(Component.For(typeof(PhotoController)).LifestylePerWebRequest());
                _container.Register(Component.For(typeof(PagesController)).LifestylePerWebRequest());
                _container.Register(Component.For(typeof(AdvController)).LifestylePerWebRequest());

                

            }

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory());

            //  Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AdvContext>());
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
            AreaRegistration.RegisterAllAreas();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
