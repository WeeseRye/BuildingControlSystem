using System.Text;

namespace BuildingControlSystem.Devices
{
    public class CardReader : Device
    {
        public override DeviceType Type => DeviceType.CardReader;
        private string _accessCardNumber;

        public string AccessCardNumber
        {
            get => _accessCardNumber;
            set
            {
                if (IsValidAccessCardNumber(value))
                {
                    _accessCardNumber = ReverseBytesAndPad(value);
                    OnDeviceChanged();
                }
                else
                {
                    throw new ArgumentException("Invalid access card number. It must be an even-length hexadecimal string with a maximum length of 16.");
                }
            }
        }
        public CardReader(string name, string cardNum) : base(name)
        {
            AccessCardNumber = cardNum;
        }

        private static bool IsValidAccessCardNumber(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length > 16 || value.Length % 2 != 0)
                return false;
            foreach (char c in value)
            {
                if (!Uri.IsHexDigit(c))
                    return false;
            }
            return true;
        }

        private static string ReverseBytesAndPad(string value)
        {
            StringBuilder sb = new(value.Length);
            for (int i = value.Length - 2; i >= 0; i -= 2)
            {
                sb.Append(value, i, 2);
            }

            string result = sb.ToString();
            result = result.PadLeft(16, '0');
            return result;
        }

        public override string GetCurrentState()
        {
            return $"AccessCardNumber= {AccessCardNumber}";
        }
    }
}
