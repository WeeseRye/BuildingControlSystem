using BuildingControlSystem.Devices;

namespace BuildingControlSystem
{
    public class Root
    {
        private List<DeviceGroup> groups = [];

        public event Action? StructureChanged;

        private void OnStructureChanged()
        {
            StructureChanged?.Invoke();
        }

        public void AddGroup(DeviceGroup group)
        {
            if (groups.Any(g => g.Name == group.Name))
            {
                Console.WriteLine("Group with this name already exists.");
                return;
            }

            groups.Add(group);
            group.StructureChanged += OnStructureChanged;
            OnStructureChanged();
        }

        public void RemoveGroup(string name)
        {
            groups.RemoveAll(g => g.Name == name);
            OnStructureChanged();
        }

        public IEnumerable<DeviceGroup> Groups => groups;

        public void PrintTree()
        {
            Console.WriteLine("\nBuilding Control System");
            foreach (var group in groups)
                group.PrintGroup();
        }

        public (DeviceGroup group, Device device)? FindDevice(int id)
        {
            foreach (var group in groups)
            {
                foreach (var device in group.Devices)
                {
                    if (device.Id == id)
                        return (group, device);
                }
            }
            return null;
        }

        public DeviceGroup? FindGroup(string name)
        {
            return groups.FirstOrDefault(g => g.Name == name);
        }
    }
}
