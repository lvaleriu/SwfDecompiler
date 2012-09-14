namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RGB : IBytes
    {
        public int Red;
        public int Green;
        public int Blue;
        public RGB(int red, int green, int blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public byte[] GetBytes
        {
            get
            {
                return new byte[] { Convert.ToByte(this.Red), Convert.ToByte(this.Green), Convert.ToByte(this.Blue) };
            }
        }
        public int Length
        {
            get
            {
                return 3;
            }
        }
    }
}

