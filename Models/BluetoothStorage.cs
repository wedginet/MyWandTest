using Microsoft.Maui.Storage;

namespace MyWandTest.Models;

public static class BluetoothStorage
{
    private const string LastDeviceKey = "LastConnectedDeviceId";

    public static void SaveLastDeviceId(string deviceId)
        => Preferences.Set(LastDeviceKey, deviceId);

    public static string? GetLastDeviceId()
        => Preferences.Get(LastDeviceKey, null);

    public static void ClearLastDevice()
        => Preferences.Remove(LastDeviceKey);
}
