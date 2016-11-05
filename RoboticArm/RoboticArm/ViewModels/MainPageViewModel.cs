using System;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace RoboticArm.ViewModels
{
	public class MainPageViewModel :ViewModelBase
	{


		public DelegateCommand NavigateCommand { get; set; }

		private BLEService _bleService;

		private string _info;

		public MainPageViewModel(INavigationService navigationService, BLEService bleService)
		{
			_navigationService = navigationService;
			_bleService = bleService;
			_info = "";
		}

		public string Info
		{
			get { return _info; }
			set { SetProperty(ref _info, value); }
		}


		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			base.OnNavigatedFrom(parameters);
		}

		public async override void OnNavigatedTo(NavigationParameters parameters)
		{
			var op = null as object;
			parameters.TryGetValue("operation", out op);
			var operation = Convert.ToString(op);
			if (operation == "red")
			{
				await _bleService.SendAsync("c1");
			}
			if (operation == "blue")
			{
				await _bleService.SendAsync("c6");
			}
			if (operation == "purple")
			{
				await _bleService.SendAsync("c7");
			}
			if (operation == "disconnect")
			{
				await _bleService.SendAsync("cc");
			}
			if (operation == "rest")
			{
				await _bleService.SendAsync("ff");
			}
			Info = Convert.ToString(op) + " " + await _bleService.GetInfo();
			base.OnNavigatedTo(parameters);
		}
	}
}


