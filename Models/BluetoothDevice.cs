namespace MyWandTest.Models
{
    public class BluetoothDevice
    {
        public string Id { get; set; } = string.Empty; // MAC address
        public string Name { get; set; } = string.Empty;

        public override string ToString() => $"{Name} ({Id})";
    }
}
