using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace RoboticArm.ViewModels
{
	public class ViewModelBase : BindableBase, INavigationAware
	{
		protected INavigationService _navigationService;
		protected bool _requiresLogin;

		public virtual void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		public virtual void OnNavigatedTo(NavigationParameters parameters)
		{
		}
	}
}
