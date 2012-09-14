namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SI8 : IBytes
    {
        private int _value;
        public SI8(int val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                AB ab = new AB(1);
                ab.Append(new SB(8, this._value));
                return ab.GetBytes;
            }
        }
        public int Length
        {
            get
            {
                return 1;
            }
        }
        public static implicit operator SI8(int x)
        {
            return new SI8(x);
        }

        public static implicit operator int(SI8 x)
        {
            return x._value;
        }
    }
}

