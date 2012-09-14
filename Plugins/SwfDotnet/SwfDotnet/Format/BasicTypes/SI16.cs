namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SI16 : IBytes
    {
        private int _value;
        public SI16(int val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                AB ab = new AB(2);
                ab.Append(new SB(0x10, this._value));
                return ab.GetBytes;
            }
        }
        public int Length
        {
            get
            {
                return 2;
            }
        }
        public static implicit operator SI16(int x)
        {
            return new SI16(x);
        }

        public static implicit operator int(SI16 x)
        {
            return x._value;
        }
    }
}

