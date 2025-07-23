using System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWandTest.Services
{
    public interface IBluetoothService
    {
        event EventHandler<string> DataReceived;
        event EventHandler<bool> ConnectionChanged;
    
        Task<IEnumerable<BluetoothDevice>> GetPairedDevicesAsync();
        Task<bool> ConnectToDeviceAsync(BluetoothDevice device);
        Task DisconnectAsync();
        BluetoothDevice? ConnectedDevice { get; }
        bool IsConnected { get; }
    
        Task<BluetoothDevice?> TryReconnectLastDeviceAsync();
    }
}
