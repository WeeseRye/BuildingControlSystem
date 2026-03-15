// See https://aka.ms/new-console-template for more information
using BuildingControlSystem;
using BuildingControlSystem.Devices;

Root root = new();
root.StructureChanged += root.PrintTree;

DeviceGroup lobby = new("Lobby");
root.AddGroup(lobby);
DeviceGroup office = new("Office");
root.AddGroup(office);
DeviceGroup parking = new("Parking");
root.AddGroup(parking);

lobby.AddDevice(new CardReader("Main Entrance Card Reader", "f91108a123"));
lobby.AddDevice(new Door("Main Entrance Door"));

office.AddDevice(new CardReader("Office Card Reader", "A1B2C3F9"));
office.AddDevice(new Door("Office Door"));
office.AddDevice(new Speaker("Office Speaker"));
office.AddDevice(new LedPanel("Office LED Panel", "foo"));

root.PrintTree();

Controller controller = new(root);

controller.Run();