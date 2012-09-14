namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FB : IBitable
    {
        private double _value;
        private int _nbits;
        public FB(int nBits, float val)
        {
            this._value = val;
            this._nbits = nBits;
        }

        public FB(int nBits, double val)
        {
            this._value = val;
            this._nbits = nBits;
        }

        public static int NumBits(double val)
        {
            int num = (int) (val * 65536.0);
            return SB.NumBits(num);
        }

        public string GetBits()
        {
            int val = (int) (this._value * 65536.0);
            SB sb = new SB(this._nbits, val);
            return sb.GetBits();
        }
    }
}

