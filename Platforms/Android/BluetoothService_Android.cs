using Android.Bluetooth;
using Android.Content;
using System.Text;
using MyWandTest.Models;
using MyWandTest.Services;
using Android.Runtime;
using Java.Util;

// Aliases to avoid type conflict
using AndroidBtDevice = Android.Bluetooth.BluetoothDevice;
using AppBluetoothDevice = MyWandTest.Models.BluetoothDevice;

namespace MyWandTest.Platforms.Android
{
    public class BluetoothService_Android : IBluetoothService
    {
        public event EventHandler<string>? DataReceived;
        public event EventHandler<bool>? ConnectionChanged;

        private BluetoothSocket? _socket;
        private CancellationTokenSource? _cts;

        public AppBluetoothDevice? ConnectedDevice { get; private set; }
        public bool IsConnected => _socket?.IsConnected == true;

        private static readonly UUID SppUuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        public async Task<IEnumerable<AppBluetoothDevice>> GetPairedDevicesAsync()
        {
            await Task.CompletedTask;
            var adapter = BluetoothAdapter.DefaultAdapter;
            var bonded = adapter?.BondedDevices;
            return bonded?.Select(d => new AppBluetoothDevice
            {
                Id = d.Address,
                Name = d.Name
            }) ?? Enumerable.Empty<AppBluetoothDevice>();
        }

        public async Task<bool> ConnectToDeviceAsync(AppBluetoothDevice device)
        {
            try
            {
                if (_socket?.IsConnected == true)
                    await DisconnectAsync();

                var adapter = BluetoothAdapter.DefaultAdapter;
                if (adapter == null || !adapter.IsEnabled)
                    throw new InvalidOperationException("Bluetooth is not enabled");

                AndroidBtDevice androidDevice = adapter.GetRemoteDevice(device.Id);
                _socket = androidDevice.CreateRfcommSocketToServiceRecord(SppUuid);

                await _socket.ConnectAsync();

                ConnectedDevice = device;
                BluetoothStorage.SaveLastDeviceId(device.Id);
                StartListening(_socket);

                ConnectionChanged?.Invoke(this, true);
                return true;
            }
            catch (Exception)
            {
                await DisconnectAsync();
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;

                _socket?.Close();
                _socket?.Dispose();
            }
            catch { }

            _socket = null;
            ConnectedDevice = null;

            ConnectionChanged?.Invoke(this, false);
            await Task.CompletedTask;
        }

        public async Task<AppBluetoothDevice?> TryReconnectLastDeviceAsync()
        {
            var lastId = BluetoothStorage.GetLastDeviceId();
            if (string.IsNullOrEmpty(lastId))
                return null;

            var devices = await GetPairedDevicesAsync();
            var match = devices.FirstOrDefault(d => d.Id == lastId);
            if (match != null && await ConnectToDeviceAsync(match))
                return match;

            return null;
        }

        private void StartListening(BluetoothSocket socket)
        {
            _cts = new CancellationTokenSource();
            var buffer = new byte[1024];

            Task.Run(async () =>
            {
                using var stream = socket.InputStream;
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        int bytes = await stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                        if (bytes > 0)
                        {
                            string message = Encoding.UTF8.GetString(buffer, 0, bytes);
                            DataReceived?.Invoke(this, message);
                        }
                    }
                    catch
                    {
                        ConnectionChanged?.Invoke(this, false);
                        break;
                    }
                }
            });
        }
    }
}
