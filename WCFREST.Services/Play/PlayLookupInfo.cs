namespace WCFREST.Services
{
	public class PlayLookupInfo
	{
		public string LookupType; // "One" or "Many". If "one", then PersonID is required
		public string PersonID;
	}
}