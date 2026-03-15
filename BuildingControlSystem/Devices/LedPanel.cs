namespace BuildingControlSystem.Devices
{
    public class LedPanel : Device
    {
        public override DeviceType Type => DeviceType.LedPanel;
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnDeviceChanged();
                }
            }
        }
        public LedPanel(string name, string message) : base(name)
        {
            _message = message;
        }
        public override string GetCurrentState()
        {
            return $"Message= {Message}";
        }
    }
}
