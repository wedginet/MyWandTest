using Android.App;                  // Android.App.Application
using Android.Content;
using Android.Bluetooth;
using AHS.Application.Bluetooth;
using AHS.Core.Bluetooth;
using Microsoft.Maui.ApplicationModel;  // for Permissions
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AHS.Maui.Platforms.Android
{
    public class BluetoothService_Android : IBluetoothService
    {
        private readonly BluetoothAdapter? _adapter;

        public BluetoothService_Android()
        {
            var btManager = (BluetoothManager?)Application
                .Context
                .GetSystemService(Context.BluetoothService);
            _adapter = btManager?.Adapter;
        }

        public async Task<IEnumerable<PairedDevice>> GetPairedDevicesAsync()
        {
            // Hardware check
            if (_adapter == null || !_adapter.IsEnabled)
                return Enumerable.Empty<PairedDevice>();

            // Android 12+ needs runtime Bluetooth permission
            var status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Bluetooth>();
                if (status != PermissionStatus.Granted)
                    return Enumerable.Empty<PairedDevice>();
            }

            // Now safe to list
            return _adapter.BondedDevices
                           .Select(d => new PairedDevice { Id = d.Address, Name = d.Name });
        }

        public async Task<bool> ConnectAsync(string deviceId)
        {
            await Task.Delay(100);
            return true;
        }
    }
}
