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
	public class CustomService
	{
		/// <summary>
		/// This method has to be called with a GET.
		///
		/// In Fiddler2 Composer, you'll have:
		/// 
		/// dropdown:	GET
		/// 
		/// address:	http://WCFREST/CustomService/GetPeople
		/// 
		/// Request Headers:
		/// 
		/// Request Body:
		///		
		/// </summary>
		[WebGet(UriTemplate = "/GetPeople",
			ResponseFormat = WebMessageFormat.Json)]
		public List<Person> GetPeople()
		{
			var retVal = new List<Person>();

			retVal.Add(new Person
			{
				PersonID = 1,
				FirstName = "John",
				LastName = "Doe",
			});
			retVal.Add(new Person
			{
				PersonID = 2,
				FirstName = "Robert",
				LastName = "Smith",
			});
			retVal.Add(new Person
			{
				PersonID = 3,
				FirstName = "Donna",
				LastName = "Red",
			});

			return retVal;
		}

		/// <summary>
		/// This method has to be called with a POST.
		/// Any paramater must be passed in as JSON in the request body.
		/// In Fiddler2 Composer, you'll have:
		/// 
		/// dropdown:	POST
		/// 
		/// address:	http://WCFREST/CustomService/GetPeopleWithPOST
		/// 
		/// Request Headers:
		///		Content-Type: application/json
		///		Content-Length: 0
		/// 
		/// Request Body:
		///		{
		///			"PersonID":"1"
		///		}
		///		
		/// </summary>
		[WebInvoke(UriTemplate = "/GetPeopleWithPOST",
			Method = "POST",
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		public List<Person> GetPeopleWithPOST(PersonLookupInfo lookupInfo)
		{
			return GetPeople().Where(p => p.FirstName.ToLower().Contains(lookupInfo.Keyword.ToLower())
				|| p.LastName.ToLower().Contains(lookupInfo.Keyword.ToLower())
				|| p.PersonID.Equals(lookupInfo.Keyword)).ToList();
		}
	}
}
