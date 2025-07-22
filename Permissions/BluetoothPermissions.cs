using Microsoft.Maui.ApplicationModel;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MyWandTest.Permissions
{
    public class BluetoothConnectPermission : BasePlatformPermission // ✅ Correct
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new[] { ("android.permission.BLUETOOTH_CONNECT", true) };
#endif
    }

    public class BluetoothScanPermission : BasePlatformPermission // ✅ Correct
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new[] { ("android.permission.BLUETOOTH_SCAN", true) };
#endif
    }
}
