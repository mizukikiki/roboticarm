using System;
using Prism.Unity;
using RoboticArm.Views;
using Xamarin.Forms;

namespace RoboticArm
{
	public partial class App : PrismApplication
	{
		protected override void OnInitialized()
		{
			NavigationService.NavigateAsync("oob://localhost/MainMasterDetail/MainNavigation/Main");
		}

		protected override void RegisterTypes()
		{
			Container.RegisterTypeForNavigation<MainMasterDetailPage>("MainMasterDetail");
			Container.RegisterTypeForNavigation<MainNavigationPage>("MainNavigation");
			Container.RegisterTypeForNavigation<MainPage>("Main");
		}
	}
}
