using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using AHS.Application.Bluetooth;
using AHS.Core.Bluetooth;
using Microsoft.Maui.ApplicationModel;      // <-- Maui Essentials
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
            var btManager = (BluetoothManager?)Application.Context
                .GetSystemService(Context.BluetoothService);

            _adapter = btManager?.Adapter;
        }

        public async Task<IEnumerable<PairedDevice>> GetPairedDevicesAsync()
        {
            // 1) Hardware & OS check
            if (_adapter == null)
                return Enumerable.Empty<PairedDevice>();

            if (!_adapter.IsEnabled)
                return Enumerable.Empty<PairedDevice>();

            // 2) Android 12+ runtime permission
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.BluetoothConnect>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.BluetoothConnect>();
                    if (status != PermissionStatus.Granted)
                    {
                        // Permission denied → nothing to list
                        return Enumerable.Empty<PairedDevice>();
                    }
                }
            }

            // 3) Now safe to enumerate
            var bonded = _adapter.BondedDevices;
            return bonded.Select(d => new PairedDevice
            {
                Id   = d.Address,
                Name = d.Name
            });
        }

        public async Task<bool> ConnectAsync(string deviceId)
        {
            // placeholder for your SPP connect logic…
            await Task.Delay(100);
            return true;
        }
    }
}
