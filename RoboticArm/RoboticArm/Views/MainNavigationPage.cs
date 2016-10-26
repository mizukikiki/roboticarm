using System;
using Xamarin.Forms;

namespace RoboticArm.Views
{
	public class MainNavigationPage : NavigationPage
	{
		public MainNavigationPage()
		{
			BarBackgroundColor = Color.FromHex("#456CD1");
			BarTextColor = Color.White;
			Title = "Robotic Arm";
		}
	}
}
