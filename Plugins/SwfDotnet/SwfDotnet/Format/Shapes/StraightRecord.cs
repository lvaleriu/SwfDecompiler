namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class StraightRecord : AB, IBoundsChanger
    {
        private bool General;
        private bool isVert;
        private int pos;
        private int posX;
        private int posY;

        public StraightRecord(bool vertical, int delta) : base(-1)
        {
            this.isVert = false;
            this.pos = 0;
            this.General = false;
            this.posX = 0;
            this.posY = 0;
            this.isVert = vertical;
            this.pos = delta;
        }

        public StraightRecord(int incX, int incY) : base(-1)
        {
            this.isVert = false;
            this.pos = 0;
            this.General = false;
            this.posX = 0;
            this.posY = 0;
            this.General = true;
            this.posX = incX;
            this.posY = incY;
        }

        protected override void OnCompile()
        {
            int maxim;
            this.Append(new UB(1, 1));
            this.Append(new UB(1, 1));
            if (this.General)
            {
                BitCounter.Init();
                BitCounter.Push(SB.NumBits(this.posX));
                BitCounter.Push(SB.NumBits(this.posY));
                maxim = BitCounter.Maxim;
            }
            else
            {
                maxim = SB.NumBits(this.pos);
            }
            this.Append(new UB(4, maxim - 2));
            this.Append(new UB(1, Convert.ToInt32( this.General)));
            if (this.General)
            {
                this.Append(new SB(maxim, this.posX));
                this.Append(new SB(maxim, this.posY));
            }
            else
            {
                this.Append(new UB(1, Convert.ToInt32( this.isVert)));
                this.Append(new SB(maxim, this.pos));
            }
        }

        public void RecordToOrig(Boundarier bounds)
        {
        }

        public void UpdateBounds(Boundarier bounds)
        {
            if (this.General)
            {
                bounds.UpdatePos(this.posX, this.posY);
            }
            else if (this.isVert)
            {
                bounds.UpdatePos(0, this.pos);
            }
            else
            {
                bounds.UpdatePos(this.pos, 0);
            }
        }
    }
}

