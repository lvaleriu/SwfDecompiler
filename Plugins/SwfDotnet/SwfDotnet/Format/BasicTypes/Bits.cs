namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Bits : IBitable
    {
        private string _val;
        public Bits(string BitArray)
        {
            this._val = BitArray;
        }

        public string GetBits()
        {
            return this._val;
        }
    }
}

