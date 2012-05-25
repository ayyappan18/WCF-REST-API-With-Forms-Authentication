using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFREST.WebAppAPI
{
	/// <summary>
	/// This is to avoid redirection to Login.ashx from FormsAuthentication.
	/// </summary>
	public class AuthRedirectModule : IHttpModule
	{
		/// <summary>
		/// You will need to configure this module in the web.config file of your
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpModule Members

		public void Dispose()
		{
			//clean-up code here.
		}

		public void Init(HttpApplication context)
		{
			// Below is an example of how you can handle LogRequest event and provide 
			// custom logging implementation for it
			//context.LogRequest += new EventHandler(OnLogRequest);
			context.EndRequest += new EventHandler(this.OnEndRequest);
		}

		#endregion

		//public void OnLogRequest(Object source, EventArgs e)
		//{
		//    //custom logging logic can go here
		//}

		private void OnEndRequest(object sender, EventArgs e)
		{
			// This is to avoid redirection to Login.ashx from FormsAuthentication.
			HttpApplication app = (HttpApplication)sender;

			if (app.Response.StatusCode == 302)
			{
				app.Response.ClearHeaders();
				app.Response.ClearContent();

				// return unauthorized
				app.Response.StatusCode = 401; // Unauthorized
			}
		}
	}
}