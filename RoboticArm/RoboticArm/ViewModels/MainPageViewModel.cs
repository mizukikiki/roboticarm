using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace RoboticArm.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		public DelegateCommand<string> ArmCommand { get; set; }

		private BLEService _bleService;

		private string _info;

		private string _color;

		private char _prefix = 'f';

		public MainPageViewModel(INavigationService navigationService, BLEService bleService)
		{
			_navigationService = navigationService;
			_bleService = bleService;
			Info = "Status: Disconnected";
			Color = "Gray";
			ArmCommand = new DelegateCommand<string>(async (o) => await ArmMove(o));
		}

		public string Info
		{
			get { return _info; }
			set { SetProperty(ref _info, value); }
		}

		public string Color
		{
			get { return _color; }
			set { SetProperty(ref _color, value); }
		}

		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			base.OnNavigatedFrom(parameters);
		}

		private async Task ArmMove(string operation)
		{
			if (operation == "X") {
				await _bleService.SendAsync(_prefix + _prefix.ToString());
			}
			else {
				await _bleService.SendAsync(_prefix + operation);
			}
		}


		public async override void OnNavigatedTo(NavigationParameters parameters)
		{
			base.OnNavigatedTo(parameters);
			var op = null as object;
			parameters.TryGetValue("operation", out op);
			var operation = Convert.ToString(op);
			if (operation == "red")
			{
				Color = "Red";
				_prefix = 'a';
			}
			if (operation == "blue")
			{
				Color = "#456CD1";
				_prefix = 'b';
			}
			if (operation == "purple")
			{
				Color = "Purple";
				_prefix = 'c';
			}
			if (operation == "disconnect")
			{
				await _bleService.SendAsync("cc");
			}
			if (operation == "rest")
			{
				await _bleService.SendAsync("ff");
			}
			Info = "Status: " + await _bleService.GetInfo();
		}

	}
}


