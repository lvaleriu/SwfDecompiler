namespace SwfDotnet.Format.BasicTypes
{
    using System;

    public class Matrix : AB
    {
        private double _angle;
        private int _objX;
        private int _objY;
        private double _rt0;
        private double _rt1;
        private double _scaleX;
        private double _scaleY;
        private double _scX;
        private double _scY;
        private double _skewX;
        private double _skewY;
        private int _tranX;
        private int _tranY;
        private int _trX;
        private int _trY;
        private bool hasRotate;
        private bool hasScale;
        private bool hasTranslation;

        public Matrix() : base(-1)
        {
            this.hasScale = false;
            this.hasRotate = false;
            this.hasTranslation = false;
            this._angle = 0.0;
            this._scaleX = 1.0;
            this._scaleY = 1.0;
            this._objX = 0;
            this._objY = 0;
            this._scX = 1.0;
            this._scY = 1.0;
        }

        private void Apply()
        {
            double num = Math.Cos((this._angle * 3.1415926535897931) / 180.0);
            double num2 = Math.Sin((this._angle * 3.1415926535897931) / 180.0);
            double num3 = this._skewX;
            double num4 = this._skewY;
            this._scX = this._scaleX * (num - (num3 * num2));
            this._rt0 = this._scaleX * ((num4 * num) - (((num3 * num4) + 1.0) * num2));
            this._rt1 = this._scaleY * (num2 + (num3 * num));
            this._scY = this._scaleY * ((num4 * num2) + (((num3 * num4) + 1.0) * num));
            this._trX = this._tranX;
            this._trY = this._tranY;
        }

        protected override void OnCompile()
        {
            this.Apply();
            if (this.hasRotate)
            {
                this.hasScale = true;
            }
            this.Append(new UB(1, Convert.ToInt32( this.hasScale)));
            if (this.hasScale)
            {
                double num = Math.Cos(this._angle);
                BitCounter.Init();
                BitCounter.Push(FB.NumBits(this._scX));
                BitCounter.Push(FB.NumBits(this._scY));
                int maxim = BitCounter.Maxim;
                this.Append(new UB(5, maxim));
                this.Append(new FB(maxim, this._scX));
                this.Append(new FB(maxim, this._scY));
            }
            this.Append(new UB(1, Convert.ToInt32(this.hasRotate)));
            if (this.hasRotate)
            {
                double num3 = Math.Sin(this._angle);
                BitCounter.Init();
                BitCounter.Push(FB.NumBits(this._rt0));
                BitCounter.Push(FB.NumBits(this._rt1));
                int val = BitCounter.Maxim;
                this.Append(new UB(5, val));
                this.Append(new FB(val, this._rt0));
                this.Append(new FB(val, this._rt1));
            }
            if (this.hasTranslation)
            {
                int num5 = this._tranX - this._objX;
                int num6 = this._tranY - this._objY;
                if (this.hasRotate)
                {
                    double num7 = Math.Cos(this._angle);
                    double num8 = this._scaleX * num7;
                    double num9 = this._scaleY * num7;
                    num5 = (this._tranX - ((int) (this._objX * num8))) + ((int) (this._objY * Math.Sin(this._angle)));
                    num6 = (this._tranY - ((int) (this._objX * Math.Sin(this._angle)))) - ((int) (this._objY * num9));
                }
                BitCounter.Init();
                BitCounter.Push(SB.NumBits(this._trX));
                BitCounter.Push(SB.NumBits(this._trY));
                int num10 = BitCounter.Maxim;
                this.Append(new UB(5, num10));
                this.Append(new SB(num10, this._trX));
                this.Append(new SB(num10, this._trY));
            }
            else
            {
                this.Append(new UB(5, 0));
            }
        }

        public void Rotate(int Angle)
        {
            this.hasRotate = true;
            this._angle = Angle;
        }

        public void Scale(double ScaleX, double ScaleY)
        {
            this.hasScale = true;
            this._scaleX = ScaleX;
            this._scaleY = ScaleY;
        }

        public void Skew(double SkewX, double SkewY)
        {
            this.hasRotate = true;
            this._skewX = SkewX;
            this._skewY = SkewY;
        }

        public void Tanslate(int X, int Y)
        {
            this.hasTranslation = true;
            this._tranX = X;
            this._tranY = Y;
        }
    }
}

