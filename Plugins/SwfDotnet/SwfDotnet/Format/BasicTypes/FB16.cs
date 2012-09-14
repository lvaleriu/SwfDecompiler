namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FB16 : IBytes
    {
        private double _value;
        public FB16(float val)
        {
            this._value = val;
        }

        public FB16(double val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                int num = (int) (this._value * 65536.0);
                return BitConverter.GetBytes(num);
            }
        }
        public int Length
        {
            get
            {
                return 4;
            }
        }
        public static implicit operator FB16(double x)
        {
            return new FB16(x);
        }

        public static implicit operator double(FB16 x)
        {
            return x._value;
        }
    }
}

