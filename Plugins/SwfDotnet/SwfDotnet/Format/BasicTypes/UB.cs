namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential)]
    public struct UB : IBitable
    {
        private int _value;
        private int _nBits;
        public UB(int nBits)
        {
            this._nBits = nBits;
            this._value = 0;
        }

        public UB(int nBits, int val)
        {
            this._nBits = nBits;
            this._value = val;
        }

        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        public string GetBits()
        {
            StringBuilder builder = new StringBuilder(Convert.ToString((long) this._value, 2).PadLeft(this._nBits, '0'));
            int length = builder.Length;
            return builder.ToString(length - this._nBits, this._nBits);
        }

        public static int NumBits(long val)
        {
            int num = 0x20;
            long num2 = 0x80000000L;
            while (num > 0)
            {
                if ((val & num2) != 0L)
                {
                    return num;
                }
                num2 = num2 >> 1;
                num--;
            }
            return 0;
        }
    }
}

