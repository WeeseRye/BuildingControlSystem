using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingControlSystem.Devices
{
    internal class Door
    {
        [Flags]
        internal enum DoorState
        {
            Locked = 1 << 0,
            Open = 1 << 1,
            OpenForTooLong = 1 << 2,
            OpenedForcibly = 1 << 3
        }
    }
}
