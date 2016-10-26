using System;
using RoboticArm.Models;
using Prism.Navigation;
using Prism.Commands;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace RoboticArm.ViewModels
{
	public class MainMasterDetailPageViewModel : BindableBase
	{
		private readonly INavigationService _navigationService;
		private string _icon = "slideout.png";
		private string _title = "RoboticArm";
		private HomeMenuItem _selectedMenuItem;

		public MainMasterDetailPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;

			MenuItems = new ObservableCollection<HomeMenuItem> {
				new HomeMenuItem {
					Title = "Rest",
					MenuType = HomeMenuType.Rest
				},
				new HomeMenuItem {
					Title = "Red Arm",
					MenuType = HomeMenuType.RedArm
				},
				new HomeMenuItem {
					Title = "Blue Arm",
					MenuType = HomeMenuType.BlueArm
				},
				new HomeMenuItem {
					Title = "Both Arms",
					MenuType = HomeMenuType.BothArms
				}
			};
			SelectCommand = new DelegateCommand(Navigate);
		}

		public ObservableCollection<HomeMenuItem> MenuItems { get; set; }

		public string Icon { get { return _icon; } }

		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		public HomeMenuItem SelectedMenuItem
		{
			get { return _selectedMenuItem; }
			set { SetProperty(ref _selectedMenuItem, value); }
		}

		public DelegateCommand SelectCommand { get; set; }

		private void Navigate()
		{
			var menuType = SelectedMenuItem?.MenuType;

			switch (menuType)
			{
				case HomeMenuType.Rest:
					_navigationService.NavigateAsync("ConfigurationNavigation/Configuration");
					break;
				case HomeMenuType.RedArm:
					_navigationService.NavigateAsync("RequestAlertNavigation/RequestAlert");
					break;
				case HomeMenuType.BlueArm:
					_navigationService.NavigateAsync("RequestAlertNavigation/RequestAlert");
					break;
				case HomeMenuType.BothArms:
					_navigationService.NavigateAsync("RequestAlertNavigation/RequestAlert");
					break;
			}
		}
	}
}

