namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UI32 : IBytes
    {
        private int _value;
        public UI32(int val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                byte[] buffer = new byte[4];
                return BitConverter.GetBytes(this._value);
            }
        }
        public int Length
        {
            get
            {
                return 4;
            }
        }
        public static implicit operator UI32(int x)
        {
            return new UI32(x);
        }

        public static implicit operator int(UI32 x)
        {
            return x._value;
        }
    }
}

