namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential)]
    public struct SB : IBitable
    {
        private int _value;
        private int _nBits;
        public SB(int nBits)
        {
            this._nBits = nBits;
            this._value = 0;
        }

        public SB(int nBits, int val)
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
            StringBuilder builder;
            long num = this._value & 0x7fffffff;
            if (this._value < 0)
            {
                num |= ((long) 1L) << (this._nBits - 1);
                builder = new StringBuilder(Convert.ToString(num, 2).PadLeft(this._nBits, '1'));
            }
            else
            {
                builder = new StringBuilder(Convert.ToString(num, 2).PadLeft(this._nBits, '0'));
            }
            int length = builder.Length;
            return builder.ToString(length - this._nBits, this._nBits);
        }

        public static int NumBits(int val)
        {
            if (val >= 0)
            {
                return (UB.NumBits((long) val) + 1);
            }
            int num = 0x1f;
            long num2 = 0x40000000L;
            while (num > 0)
            {
                if ((val & num2) == 0L)
                {
                    break;
                }
                num2 = num2 >> 1;
                num--;
            }
            if (num == 0)
            {
                return 2;
            }
            if ((val & ((((int) 1) << num) - 1)) == 0)
            {
                num++;
            }
            return (num + 1);
        }
    }
}

