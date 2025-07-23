using MyWandTest.Models;

namespace MyWandTest.Services
{
    public interface IBluetoothService
    {
        event EventHandler<string> DataReceived;
        event EventHandler<bool> ConnectionChanged;

        Task<IEnumerable<BluetoothDevice>> GetPairedDevicesAsync();
        Task<bool> ConnectToDeviceAsync(BluetoothDevice device);
        Task DisconnectAsync();
        Task<BluetoothDevice?> TryReconnectLastDeviceAsync();

        BluetoothDevice? ConnectedDevice { get; }
        bool IsConnected { get; }
    }
}
