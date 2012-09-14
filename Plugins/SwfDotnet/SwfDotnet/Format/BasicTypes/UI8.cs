namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UI8 : IBytes
    {
        public int _value;
        public UI8(int val)
        {
            this._value = val;
        }

        public UI8(char val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                byte[] buffer = new byte[1];
                byte num = Convert.ToByte(this._value);
                buffer[0] = BitConverter.GetBytes((short) num)[0];
                return buffer;
            }
        }
        public int Length
        {
            get
            {
                return 1;
            }
        }
        public static implicit operator UI8(int x)
        {
            return new UI8(x);
        }

        public static implicit operator int(UI8 x)
        {
            return x._value;
        }
    }
}

