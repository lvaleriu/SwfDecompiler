namespace SwfDotnet.Shapes
{
    using SwfDotnet.Format.Shapes;
    using SwfDotnet.Format.Tags;
    using System;

    public class Rectangle : DefineShape
    {
        private SwfDotnet.Format.Shapes.FillStyle _fillStyle;
        private SwfDotnet.Format.Shapes.LineStyle _lineStyle;
        private int Curve = 800;
        private int cx;
        private int cy;

        public Rectangle(int width, int height, int roundCornerRadius)
        {
            this.cx = width;
            this.cy = height;
            this.Curve = roundCornerRadius;
        }

        protected override void OnCompile()
        {
            if (this._lineStyle != null)
            {
                base.LineStyles.Add(this._lineStyle);
            }
            if (this._fillStyle != null)
            {
                base.FillStyles.Add(this._fillStyle);
            }
            base.Records.Add(new StyleChangeRecord(this.Curve, this.Curve, 1, 1, 0));
            base.Records.Add(new StraightRecord(false, this.cx - (this.Curve * 2)));
            if (this.Curve > 0)
            {
                base.Records.Add(new CurvedRecord(this.Curve, 0, 0, this.Curve));
            }
            base.Records.Add(new StraightRecord(true, this.cy - (this.Curve * 2)));
            if (this.Curve > 0)
            {
                base.Records.Add(new CurvedRecord(0, this.Curve, -this.Curve, 0));
            }
            base.Records.Add(new StraightRecord(false, -(this.cx - (this.Curve * 2))));
            if (this.Curve > 0)
            {
                base.Records.Add(new CurvedRecord(-this.Curve, 0, 0, -this.Curve));
            }
            base.Records.Add(new StraightRecord(true, -(this.cy - (this.Curve * 2))));
            if (this.Curve > 0)
            {
                base.Records.Add(new CurvedRecord(0, -this.Curve, this.Curve, 0));
            }
            base.OnCompile();
        }

        public SwfDotnet.Format.Shapes.FillStyle FillStyle
        {
            get
            {
                return this._fillStyle;
            }
            set
            {
                this._fillStyle = value;
            }
        }

        public SwfDotnet.Format.Shapes.LineStyle LineStyle
        {
            get
            {
                return this._lineStyle;
            }
            set
            {
                this._lineStyle = value;
            }
        }
    }
}

