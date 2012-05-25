using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace WCFREST.Services
{
	/// <summary>
	/// This is a WCF Service implemented just as a class, without having to use a .svc file.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class PlayService
	{
		/// <summary>
		/// This method has to be called with a GET.
		///
		/// In Fiddler2 Composer, you'll have:
		/// dropdown:	GET
		/// address:	http://localhost:65432/PlayService/GetAllPositions
		/// Request Headers:
		/// 
		/// Request Body:
		///		
		/// </summary>
		[WebGet(UriTemplate = "/GetAllPositions",
			ResponseFormat = WebMessageFormat.Json)]
		public List<PlayPosition> GetAllPositions()
		{
			return DemoData.List;
		}

		/// <summary>
		/// This method has to be called with a POST.
		/// Any paramater must be passed in as JSON in the request body.
		/// In Fiddler2 Composer, you'll have:
		/// dropdown:	POST
		/// address:	http://localhost:65432/PlayService/GetPosition
		/// Request Headers:
		///		Content-Type: application/json
		///		Content-Length: 0
		/// 
		/// Request Body:
		///		{
		///			"PersonID":"1"
		///		}
		/// </summary>
		[WebInvoke(UriTemplate = "/GetPosition",
			Method = "POST",
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		public List<PlayPosition> GetPosition(PlayLookupInfo lookupInfo)
		{
			if (lookupInfo.LookupType == "One")
			{
				var list = DemoData.List.Where(p =>
					p.PersonID.ToString().Equals(lookupInfo.PersonID)
				).ToList();
				return list;
			}
			else // "Many"
			{
				return DemoData.List;
			}
		}

		[WebInvoke(UriTemplate = "/ReceivePosition",
			Method = "POST",
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		public List<PlayPosition> ReceivePosition(PlayPosition position)
		{
			var existing = DemoData.List.Where(p =>
				p.PersonID == position.PersonID).FirstOrDefault();
			if (existing != null)
			{
				existing.X = position.X;
				existing.Y = position.Y;
			}
			else
			{
				DemoData.List.Add(position);
			}
			return DemoData.List;
		}
	}
}
