namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UI16 : IBytes
    {
        private int _value;
        public UI16(int val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                byte[] buffer = new byte[2];
                return BitConverter.GetBytes(Convert.ToInt16(this._value));
            }
        }
        public int Length
        {
            get
            {
                return 2;
            }
        }
        public static implicit operator UI16(int x)
        {
            return new UI16(x);
        }

        public static implicit operator int(UI16 x)
        {
            return x._value;
        }
    }
}

