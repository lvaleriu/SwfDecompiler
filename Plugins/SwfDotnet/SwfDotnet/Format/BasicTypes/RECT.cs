namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT : IBytes
    {
        private int bitSize;
        private int minX;
        private int minY;
        private int maxX;
        private int maxY;
        public int MinX
        {
            get
            {
                return this.minX;
            }
            set
            {
                this.minX = value;
                this.bitSize = -1;
            }
        }
        public int MinY
        {
            get
            {
                return this.minY;
            }
            set
            {
                this.minY = value;
                this.bitSize = -1;
            }
        }
        public int MaxX
        {
            get
            {
                return this.maxX;
            }
            set
            {
                this.maxX = value;
                this.bitSize = -1;
            }
        }
        public int MaxY
        {
            get
            {
                return this.maxY;
            }
            set
            {
                this.maxY = value;
                this.bitSize = -1;
            }
        }
        public int Width
        {
            get
            {
                return (this.maxX - this.minX);
            }
        }
        public int Height
        {
            get
            {
                return (this.maxY - this.minY);
            }
        }
        private int BitSize
        {
            get
            {
                if (this.bitSize == -1)
                {
                    BitCounter.Init();
                    BitCounter.Push(SB.NumBits(this.minX));
                    BitCounter.Push(SB.NumBits(this.maxX));
                    BitCounter.Push(SB.NumBits(this.minY));
                    BitCounter.Push(SB.NumBits(this.maxY));
                    this.bitSize = BitCounter.Maxim;
                }
                return this.bitSize;
            }
        }
        public int Length
        {
            get
            {
                return (int) Math.Ceiling(((double) ((this.BitSize * 4) + 5)) / 8.0);
            }
        }
        public byte[] GetBytes
        {
            get
            {
                int bitSize = this.BitSize;
                AB ab = new AB(this.Length);
                ab.Append(new UB(5, bitSize));
                ab.Append(new SB(bitSize, this.minX));
                ab.Append(new SB(bitSize, this.maxX));
                ab.Append(new SB(bitSize, this.minY));
                ab.Append(new SB(bitSize, this.maxY));
                return ab.GetBytes;
            }
        }
        public RECT(int minX, int minY, int maxX, int maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.bitSize = -1;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "RECT bitsize=", this.BitSize, " (", this.minX, ",", this.minY, ")-(", this.maxX, ",", this.maxY, ")" });
        }
    }
}

