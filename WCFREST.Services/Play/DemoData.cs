using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFREST.Services
{
	public static class DemoData
	{
		private static List<PlayPosition> _list = null;

		public static List<PlayPosition> List
		{
			get
			{
				if (_list == null)
				{
					_list = new List<PlayPosition>();

					_list.Add(new PlayPosition
					{
						PersonID = 1,
						X = "50",
						Y = "20",
					});
					_list.Add(new PlayPosition
					{
						PersonID = 2,
						X = "100",
						Y = "20",
					});
					_list.Add(new PlayPosition
					{
						PersonID = 3,
						X = "150",
						Y = "20",
					});
				}

				return _list;
			}
		}
	}
}
