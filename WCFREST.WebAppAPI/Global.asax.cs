using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

using WCFREST.Services;

namespace WCFREST.WebAppAPI
{
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// We need to register a System.ServiceModel.Activation.ServiceRoute for each custom service.
		/// </summary>
		private void RegisterRoutes()
		{
			// The route prefix will be the root part of the URL for your service.
			// So in this case, the URL for this service will be: http://WCFREST/CustomService
			// And you can call the GetPeople method like this: http://WCFREST/CustomService/GetPeople
			// because in the CustomService.cs GetPeople method we specified WebGet UriTemplate as "/GetPeople"
			RouteTable.Routes.Add(new ServiceRoute("CustomService", new WebServiceHostFactory(), typeof(CustomService)));
		}
		
		protected void Application_Start(object sender, EventArgs e)
		{
			RegisterRoutes();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}