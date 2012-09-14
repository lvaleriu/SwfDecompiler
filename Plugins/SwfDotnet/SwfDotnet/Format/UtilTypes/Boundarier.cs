namespace SwfDotnet.Format.UtilTypes
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class Boundarier
    {
        public int CurrX = 0;
        public int CurrY = 0;
        public int difX = 0;
        public int difY = 0;
        private bool Empty = true;
        private int X1 = 0;
        private int X2 = 0;
        private int Y1 = 0;
        private int Y2 = 0;

        public void MoveToOrig()
        {
            this.difX = this.X1;
            this.difY = this.Y1;
            this.X1 -= this.difX;
            this.X2 -= this.difX;
            this.Y1 -= this.difY;
            this.Y2 -= this.difY;
        }

        public void NewPos(int AbsX, int AbsY)
        {
            if (this.Empty)
            {
                this.X1 = this.X2 = AbsX;
                this.Y1 = this.Y2 = AbsY;
                this.Empty = false;
            }
            else
            {
                if (AbsX < this.X1)
                {
                    this.X1 = AbsX;
                }
                if (AbsX > this.X2)
                {
                    this.X2 = AbsX;
                }
                if (AbsY < this.Y1)
                {
                    this.Y1 = AbsY;
                }
                if (AbsY > this.Y2)
                {
                    this.Y2 = AbsY;
                }
            }
            this.CurrX = AbsX;
            this.CurrY = AbsY;
        }

        public void OpenRect(int Increment)
        {
            this.X1 -= Increment;
            this.Y1 -= Increment;
            this.X2 += Increment;
            this.Y2 += Increment;
        }

        public void UpdatePos(int IncX, int IncY)
        {
            this.CurrX += IncX;
            this.CurrY += IncY;
            if (this.CurrX < this.X1)
            {
                this.X1 = this.CurrX;
            }
            if (this.CurrX > this.X2)
            {
                this.X2 = this.CurrX;
            }
            if (this.CurrY < this.Y1)
            {
                this.Y1 = this.CurrY;
            }
            if (this.CurrY > this.Y2)
            {
                this.Y2 = this.CurrY;
            }
        }

        public RECT Bounds
        {
            get
            {
                return new RECT(this.X1, this.Y1, this.X2, this.Y2);
            }
        }
    }
}

