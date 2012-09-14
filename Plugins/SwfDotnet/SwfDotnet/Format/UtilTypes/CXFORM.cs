namespace SwfDotnet.Format.UtilTypes
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class CXFORM : AB
    {
        private int aB;
        private int aG;
        private int aR;
        private bool hasAdd;
        private bool hasMult;
        private int mB;
        private int mG;
        private int mR;

        public CXFORM(RGB AddColor, RGB MultColor) : base(-1)
        {
            this.hasAdd = false;
            this.hasMult = false;
            this.hasAdd = true;
            this.hasMult = true;
            this.aR = AddColor.Red;
            this.aG = AddColor.Green;
            this.aB = AddColor.Blue;
            this.mR = MultColor.Red;
            this.mG = MultColor.Green;
            this.mB = MultColor.Blue;
        }

        protected override void OnCompile()
        {
            this.Append(new UB(1, Convert.ToInt32( this.hasAdd)));
            this.Append(new UB(1, Convert.ToInt32( this.hasMult)));
            BitCounter.Init();
            if (this.hasAdd)
            {
                BitCounter.Push(SB.NumBits(this.aR));
                BitCounter.Push(SB.NumBits(this.aG));
                BitCounter.Push(SB.NumBits(this.aB));
            }
            if (this.hasMult)
            {
                BitCounter.Push(SB.NumBits(this.mR));
                BitCounter.Push(SB.NumBits(this.mG));
                BitCounter.Push(SB.NumBits(this.mB));
            }
            int maxim = BitCounter.Maxim;
            this.Append(new UB(4, maxim));
            if (this.hasAdd)
            {
                this.Append(new SB(maxim, this.aR));
                this.Append(new SB(maxim, this.aG));
                this.Append(new SB(maxim, this.aB));
            }
            if (this.hasMult)
            {
                this.Append(new SB(maxim, this.mR));
                this.Append(new SB(maxim, this.mG));
                this.Append(new SB(maxim, this.mB));
            }
            base.OnCompile();
        }
    }
}

