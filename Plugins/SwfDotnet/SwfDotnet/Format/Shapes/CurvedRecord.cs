namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class CurvedRecord : AB, IBoundsChanger
    {
        public int anchX;
        public int anchY;
        public int posX;
        public int posY;

        public CurvedRecord(double cx, double cy, double ax, double ay) : base(-1)
        {
            this.posX = (int) cx;
            this.posY = (int) cy;
            this.anchX = (int) ax;
            this.anchY = (int) ay;
        }

        public CurvedRecord(int cx, int cy, int ax, int ay) : base(-1)
        {
            this.posX = cx;
            this.posY = cy;
            this.anchX = ax;
            this.anchY = ay;
        }

        protected override void OnCompile()
        {
            this.Append(new UB(1, 1));
            this.Append(new UB(1, 0));
            BitCounter.Init();
            BitCounter.Push(SB.NumBits(this.posX));
            BitCounter.Push(SB.NumBits(this.posY));
            BitCounter.Push(SB.NumBits(this.anchX));
            BitCounter.Push(SB.NumBits(this.anchY));
            int maxim = BitCounter.Maxim;
            this.Append(new UB(4, maxim - 2));
            this.Append(new SB(maxim, this.posX));
            this.Append(new SB(maxim, this.posY));
            this.Append(new SB(maxim, this.anchX));
            this.Append(new SB(maxim, this.anchY));
        }

        public void RecordToOrig(Boundarier bounds)
        {
        }

        public void UpdateBounds(Boundarier bounds)
        {
            bounds.UpdatePos(this.posX, this.posY);
            bounds.UpdatePos(this.anchX, this.anchY);
        }
    }
}

