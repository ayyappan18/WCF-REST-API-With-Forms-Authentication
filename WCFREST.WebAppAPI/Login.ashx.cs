using System;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace WCFREST.WebAppAPI
{
	/// <summary>
	/// Before making calls to this API, a call to this handler is needed to get a cookie.
	/// </summary>
	public class Login : IHttpHandler
	{
		/// <summary>
		/// Process the incoming Login request and return a cookie if successful.
		/// Also return a json body with a Success flag.
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			string jsonInput = context.Request.InputStream.StreamToString();
			JavaScriptSerializer s = new JavaScriptSerializer();
			var loginInfo = (LoginInfo)s.Deserialize(jsonInput, typeof(LoginInfo));

			bool success = false;
			if (loginInfo != null)
			{
				HttpCookie cookie = null;

				success = AuthenticateAndGetCookie(loginInfo, out cookie);
				if (success && cookie != null)
				{
					context.Response.Cookies.Add(cookie);
				}
			}

			string jsonOutput = null;
			if (success == false)
			{
				jsonOutput = "{\"Success\":\"False\"}";
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				context.Response.AddHeader(@"Success", @"False");
			}
			else
			{
				jsonOutput = "{\"Success\":\"True\"}";
			}

			context.Response.ContentType = "application/json";
			context.Response.Write(jsonOutput);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		protected bool AuthenticateAndGetCookie(LoginInfo loginInfo, out HttpCookie cookie)
		{
			bool returnValue = false;
			cookie = null;

			// TODO: Need to perform authentication against AD or db here
			// for this demo, hard code
			returnValue = loginInfo.UserName == "me@you.com" && loginInfo.Password == "pw";

			if (returnValue)
			{
				var ticket = new FormsAuthenticationTicket(
						1,
						loginInfo.UserName,
						DateTime.Now,
						DateTime.Now.AddMinutes(5),
						true,
						loginInfo.UserName // or user.UserID or other info you might find appropriate
					);
				string encryptedTicket = FormsAuthentication.Encrypt(ticket);
				cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
			}

			return returnValue;
		}
	}
}