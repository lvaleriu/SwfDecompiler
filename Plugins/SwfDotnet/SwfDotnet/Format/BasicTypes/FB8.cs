namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FB8 : IBytes
    {
        private double _value;
        public FB8(float val)
        {
            this._value = val;
        }

        public FB8(double val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                return BitConverter.GetBytes(Convert.ToInt16((double) (this._value * 256.0)));
            }
        }
        public int Length
        {
            get
            {
                return 2;
            }
        }
        public static implicit operator FB8(double x)
        {
            return new FB8(x);
        }

        public static implicit operator double(FB8 x)
        {
            return x._value;
        }
    }
}

