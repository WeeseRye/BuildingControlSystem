using System;
using System.Collections.Generic;
using System.Text;

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
            Console.WriteLine(GetCurrentState());
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
