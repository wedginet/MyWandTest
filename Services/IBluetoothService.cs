using System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWandTest.Services
{
    public interface IBluetoothService
    {
        Task<IEnumerable<string>> GetPairedDeviceNamesAsync();
        Task<bool> ConnectToDeviceAsync(string deviceName);
        event EventHandler<string>? DataReceived;
        void Disconnect();
    }
}