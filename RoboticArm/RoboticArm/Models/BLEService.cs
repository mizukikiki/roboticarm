using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Linq;

namespace RoboticArm
{
	public class BLEService
	{
		private IBluetoothLE _ble;
		private IAdapter _adapter;
		private ICharacteristic _characteristic;

		IDevice _device = null;

		public BLEService()
		{
			_ble = CrossBluetoothLE.Current;
			_adapter = CrossBluetoothLE.Current.Adapter;
			_adapter.DeviceDiscovered += (s, a) =>
			{
				if (a.Device.Name == "echo" || a.Device.Name == "raspberrypi")
				{
					_device = a.Device;
				}
			};
		}

		public async Task SendAsync(string message)
		{
			if (_characteristic != null)
			{
				byte[] x = null;
				await _characteristic.WriteAsync(x);
			}
		}

		public async Task<string> GetInfo()
		{
			if (_device != null)
			{
				await _adapter.ConnectToDeviceAsync(_device);
				var services = await _device.GetServicesAsync();
				if (services.Count > 0)
				{
					var characteristics = await services[0].GetCharacteristicsAsync();
					if (characteristics != null && characteristics.Count() > 0)
					{
						_characteristic = characteristics.First();
					}

				}
			}
			else {
				await _adapter.StartScanningForDevicesAsync();
			}
			return "WTF " + _ble.State + " " + _device.Name;
		}
	}
}
