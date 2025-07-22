using static Microsoft.Maui.ApplicationModel.Permissions;
using Microsoft.Maui.Devices;
using MyWandTest.Permissions; // ✅ Reference your custom permission classes

namespace MyWandTest.Services
{
    public static class PermissionsHelper
    {
        public static async Task<bool> RequestBluetoothPermissionsAsync()
        {
#if ANDROID
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Request Bluetooth Connect
                var connectStatus = await CheckStatusAsync<BluetoothConnectPermission>();
                if (connectStatus != PermissionStatus.Granted)
                {
                    connectStatus = await RequestAsync<BluetoothConnectPermission>();
                }

                // Request Bluetooth Scan
                var scanStatus = await CheckStatusAsync<BluetoothScanPermission>();
                if (scanStatus != PermissionStatus.Granted)
                {
                    scanStatus = await RequestAsync<BluetoothScanPermission>();
                }
                System.Diagnostics.Debug.WriteLine($"Connect: {connectStatus}");
                System.Diagnostics.Debug.WriteLine($"Scan: {scanStatus}");

                return connectStatus == PermissionStatus.Granted &&
                       scanStatus == PermissionStatus.Granted;
            }
#endif
            await Task.CompletedTask;
            return true; // Assume permission granted for non-Android platforms
        }
    }
}
