using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingControlSystem.Devices
{
    public class Door : Device
    {
        [Flags]
        public enum DoorState
        {
            None = 0,
            Locked = 1 << 0,
            Open = 1 << 1,
            OpenForTooLong = 1 << 2,
            OpenedForcibly = 1 << 3
        }

        public override DeviceType Type => DeviceType.Door;

        public DoorState State { get; private set; }

        public bool Locked
        {
            get => (State & DoorState.Locked) != 0;
            set => SetFlag(DoorState.Locked, value);
        }

        public bool Open
        {
            get => (State & DoorState.Open) != 0;
            set => SetFlag(DoorState.Open, value);
        }

        public bool OpenForTooLong
        {
            get => (State & DoorState.OpenForTooLong) != 0;
            set => SetFlag(DoorState.OpenForTooLong, value);
        }

        public bool OpenedForcibly
        {
            get => (State & DoorState.OpenedForcibly) != 0;
            set => SetFlag(DoorState.OpenedForcibly, value);
        }

        public Door(string name) : base(name) { }

        private void SetFlag(DoorState flag, bool value)
        {
            if (value)
                State |= flag;
            else
                State &= ~flag;
            OnDeviceChanged();
        }

        public override string GetCurrentState()
        {
            return State.ToString();
        }
    }
}
