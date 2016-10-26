using System;
using Prism.Navigation;
using Prism.Mvvm;

namespace RoboticArm.ViewModels
{
	public class MainNavigationPageViewModel : BindableBase, INavigationAware
	{
		INavigationService _navigationService;

		public MainNavigationPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
		}
	}
}
