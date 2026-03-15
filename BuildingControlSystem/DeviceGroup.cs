using BuildingControlSystem.Devices;

namespace BuildingControlSystem
{
    public class DeviceGroup(string name)
    {
        private string name = name;
        private readonly List<Device> devices = [];

        public event Action? StructureChanged;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnStructureChanged();
            }
        }

        public IEnumerable<Device> Devices => devices;

        public void AddDevice(Device device)
        {
            devices.Add(device);
            OnStructureChanged();
        }

        public void RemoveDevice(int id)
        {
            devices.RemoveAll(d => d.Id == id);
            OnStructureChanged();
        }

        private void OnStructureChanged()
        {
            StructureChanged?.Invoke();
        }

        public void PrintGroup()
        {
            Console.WriteLine($"   {Name}");
            foreach (var device in devices)
                Console.WriteLine($"      [{device.Id}] {device.Type} - {device.Name}: {device.GetCurrentState()}");
        }
    }
}
