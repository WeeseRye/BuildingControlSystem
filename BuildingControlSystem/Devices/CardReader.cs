using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingControlSystem.Devices
{
    /*
    Zařízení typu CardReader bude mít vlastnost AccessCardNumber typu string. Při pokusu
    o zápis její hodnoty nejprve proběhne kontrola, zdali je délka řetězce sudá, ne větší než
    16, a zda obsahuje pouze hexadecimální číslice. Byl-li předán nevyhovující řetězec, dojde
    k chybě, jinak se předá transformační funkci ReverseBytesAndPad a její výsledek se uloží
    jako hodnota vlastnosti AccessCardNumber. ReverseBytesAndPad je funkce jejíž jediný
    parametr i návratová hodnota jsou typu string, která převrátí pořadí bytů (tedy dvojic
    znaků, pořadí znaků ve dvojicích zůstane zachováno) v řetězci, a pokud je kratší než 16
    znaků, doplní ho zleva samými nulami do délky 16. Př.: "A01234DE7FFF" ->
    "0000FF7FDE3412A0"
    */
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
        public CardReader(string name) : base(name) { }

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
            return $"AccessCardNumber: {AccessCardNumber}";
        }
    }
}
