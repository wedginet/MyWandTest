using Android.Bluetooth;
using Java.Util;
using MyWandTest.Services;
using System.Text;

namespace MyWandTest.Platforms.Android
{
    public class BluetoothService_Android : IBluetoothService
    {
        private BluetoothSocket? _socket;
        private CancellationTokenSource? _cts;
        private readonly UUID _sppUuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        public event EventHandler<string>? DataReceived;

        public async Task<IEnumerable<string>> GetPairedDeviceNamesAsync()
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null || !adapter.IsEnabled)
                return Enumerable.Empty<string>();

            return adapter.BondedDevices.Select(d => d.Name);
        }

        public async Task<bool> ConnectToDeviceAsync(string deviceName)
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            var device = adapter.BondedDevices.FirstOrDefault(d => d.Name == deviceName);

            if (device == null) return false;

            _socket = device.CreateRfcommSocketToServiceRecord(_sppUuid);
            await _socket.ConnectAsync();

            _cts = new CancellationTokenSource();
            StartListening(_cts.Token);

            return true;
        }

        private void StartListening(CancellationToken token)
        {
            Task.Run(() =>
            {
                try
                {
                    var buffer = new byte[1024];
                    while (!token.IsCancellationRequested && _socket?.InputStream != null)
                    {
                        int bytes = _socket.InputStream.Read(buffer, 0, buffer.Length);
                        if (bytes > 0)
                        {
                            var received = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                            MainThread.BeginInvokeOnMainThread(() => DataReceived?.Invoke(this, received));
                        }
                    }
                }
                catch { /* swallow or log */ }
            });
        }

        public void Disconnect()
        {
            _cts?.Cancel();
            _socket?.Close();
            _socket = null;
        }
    }
}
