namespace BuildingControlSystem.Devices
{
    public enum DeviceType
    {
        Door,
        Speaker,
        LedPanel,
        Alarm,
        CardReader
    }

    public abstract class Device
    {
        private static int nextId = 1;
        public int Id { get; }
        public string Name { get; set; }
        public abstract DeviceType Type { get; }

        protected void OnDeviceChanged()
        {
            Console.WriteLine($"\nUpdated device -> [{Id}] {Type} - {Name}: {GetCurrentState()}");
        }

        protected Device(string name)
        {
            Id = nextId++;
            Name = name;
        }

        public virtual string GetCurrentState()
        {
            return "";
        }
    }
}
