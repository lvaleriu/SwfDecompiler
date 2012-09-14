namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBA : IBytes
    {
        public int Red;
        public int Green;
        public int Blue;
        public int Alpha;
        public RGBA(int red, int green, int blue, int alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        public byte[] GetBytes
        {
            get
            {
                return new byte[] { Convert.ToByte(this.Red), Convert.ToByte(this.Green), Convert.ToByte(this.Blue), Convert.ToByte(this.Alpha) };
            }
        }
        public int Length
        {
            get
            {
                return 4;
            }
        }
    }
}

