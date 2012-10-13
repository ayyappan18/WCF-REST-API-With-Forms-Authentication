using System;
using System.Text;

namespace WCFREST.WebAppAPI
{
	/// <summary>
	/// TODO: This should be moved into solution CommonDOTNET, library Base.ExtensionMethods
	/// </summary>
	public static class StreamExtensions
	{
		public static string StreamToString(this System.IO.Stream s)
		{
			StringBuilder sb = new StringBuilder();

			int streamLength = Convert.ToInt32(s.Length);
			Byte[] streamArray = new Byte[streamLength];

			int streamRead = s.Read(streamArray, 0, streamLength);

			for (int i = 0; i < streamLength; i++)
			{
				sb.Append(Convert.ToChar(streamArray[i]));
			}

			return sb.ToString();
		}
	}
}