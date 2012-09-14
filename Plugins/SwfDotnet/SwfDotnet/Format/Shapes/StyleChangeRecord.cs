namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class StyleChangeRecord : AB, IBoundsChanger
    {
        private int FillIndex0;
        private int FillIndex1;
        private int LineIndex;
        private bool MoveTo;
        private int MoveX;
        private int MoveY;
        public IShapeData Parent;

        public StyleChangeRecord(int lineIndex) : base(-1)
        {
            this.LineIndex = 0;
            this.FillIndex0 = 0;
            this.FillIndex1 = 0;
            this.MoveTo = false;
            this.MoveX = 0;
            this.MoveY = 0;
            this.LineIndex = lineIndex;
        }

        public StyleChangeRecord(int MoveToX, int MoveToY) : base(-1)
        {
            this.LineIndex = 0;
            this.FillIndex0 = 0;
            this.FillIndex1 = 0;
            this.MoveTo = false;
            this.MoveX = 0;
            this.MoveY = 0;
            this.MoveX = MoveToX;
            this.MoveY = MoveToY;
            this.MoveTo = true;
        }

        public StyleChangeRecord(int lineIndex, int fillIndex0, int fillIndex1) : base(-1)
        {
            this.LineIndex = 0;
            this.FillIndex0 = 0;
            this.FillIndex1 = 0;
            this.MoveTo = false;
            this.MoveX = 0;
            this.MoveY = 0;
            this.LineIndex = lineIndex;
            this.FillIndex0 = fillIndex0;
            this.FillIndex1 = fillIndex1;
        }

        public StyleChangeRecord(int MoveToX, int MoveToY, int lineIndex, int fillIndex0, int fillIndex1) : base(-1)
        {
            this.LineIndex = 0;
            this.FillIndex0 = 0;
            this.FillIndex1 = 0;
            this.MoveTo = false;
            this.MoveX = 0;
            this.MoveY = 0;
            this.LineIndex = lineIndex;
            this.FillIndex0 = fillIndex0;
            this.FillIndex1 = fillIndex1;
            this.MoveX = MoveToX;
            this.MoveY = MoveToY;
            this.MoveTo = true;
        }

        protected override void OnCompile()
        {
            int val = 0;
            int num2 = 0;
            int num3 = 0;
            if (this.LineIndex > 0)
            {
                val = 1;
            }
            if (this.FillIndex0 > 0)
            {
                num2 = 1;
            }
            if (this.FillIndex1 > 0)
            {
                num3 = 1;
            }
            this.Append(new UB(1, 0));
            this.Append(new UB(1, 0));
            this.Append(new UB(1, val));
            this.Append(new UB(1, num2));
            this.Append(new UB(1, num3));
            this.Append(new UB(1, Convert.ToInt32( this.MoveTo)));
            if (this.MoveTo)
            {
                BitCounter.Init();
                BitCounter.Push(SB.NumBits(this.MoveX));
                BitCounter.Push(SB.NumBits(this.MoveY));
                int maxim = BitCounter.Maxim;
                this.Append(new UB(5, maxim));
                this.Append(new SB(maxim, this.MoveX));
                this.Append(new SB(maxim, this.MoveY));
            }
            if (num2 == 1)
            {
                this.Append(new UB(this.Parent.NumFillBits, this.FillIndex0));
            }
            if (num3 == 1)
            {
                this.Append(new UB(this.Parent.NumFillBits, this.FillIndex1));
            }
            if (val == 1)
            {
                this.Append(new UB(this.Parent.NumLineBits, this.LineIndex));
            }
        }

        public void RecordToOrig(Boundarier bounds)
        {
            this.MoveX -= bounds.difX;
            this.MoveY -= bounds.difY;
            if (!this.MoveTo)
            {
                this.MoveTo = (bounds.difX != 0) || (bounds.difY != 0);
            }
        }

        public void UpdateBounds(Boundarier bounds)
        {
            if (this.MoveTo)
            {
                bounds.NewPos(this.MoveX, this.MoveY);
            }
        }
    }
}

