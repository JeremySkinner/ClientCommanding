using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ClientCommanding.Commands;
using StructureMap;

namespace ClientCommanding {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");

			// Route the root of the site to homecontroller
			routes.MapRoute("homepage", "", new { controller = "Home", action = "Index" });

			//everything else goes to the commanding layer
			routes.MapRoute("commands", "{command}", new { controller = "CommandInvoker", action = "Invoke" });

		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);


			ObjectFactory.Configure(cfg => {
				cfg.Scan(scan => {
					scan.TheCallingAssembly();
					scan.IncludeNamespaceContainingType<GetUser>();
					scan.ConnectImplementationsToTypesClosing(typeof(CommandHandler<>));
				});
			});
		}
	}
}