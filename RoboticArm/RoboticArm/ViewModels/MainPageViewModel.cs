using System;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace RoboticArm.ViewModels
{
	public class MainPageViewModel :ViewModelBase
	{
		public DelegateCommand NavigateCommand { get; set; }

		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			base.OnNavigatedFrom(parameters);
		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			base.OnNavigatedTo(parameters);
		}
	}
}


