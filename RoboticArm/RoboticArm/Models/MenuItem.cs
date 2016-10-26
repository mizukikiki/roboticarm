using System;

namespace RoboticArm.Models
{
	public enum HomeMenuType
	{
		Rest,
		RedArm,
		BlueArm,
		BothArms
	}

	public class HomeMenuItem
	{
		public HomeMenuItem()
		{
			MenuType = HomeMenuType.Rest;
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Details { get; set; }
		public HomeMenuType MenuType { get; set; }
	}
}

