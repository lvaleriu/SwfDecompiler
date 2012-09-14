namespace SwfDotnet.Shapes
{
    using SwfDotnet.Format.Shapes;
    using SwfDotnet.Format.Tags;
    using System;

    public class Ellipse : DefineShape
    {
        private SwfDotnet.Format.Shapes.FillStyle _fillStyle;
        private SwfDotnet.Format.Shapes.LineStyle _lineStyle;
        private int cx;
        private int cy;

        public Ellipse(int width, int height)
        {
            this.cx = width;
            this.cy = height;
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
            base.Records.Add(new StyleChangeRecord(0, 0, 1, 1, 0));
            base.Records.Add(new CurvedRecord(-this.cx * 0.146, this.cy * 0.146, -this.cx * 0.206, 0.0));
            base.Records.Add(new CurvedRecord(-this.cx * 0.206, 0.0, -this.cx * 0.146, -this.cy * 0.146));
            base.Records.Add(new CurvedRecord(-this.cx * 0.146, -this.cy * 0.146, 0.0, -this.cy * 0.206));
            base.Records.Add(new CurvedRecord(0.0, -this.cy * 0.206, this.cx * 0.146, -this.cy * 0.146));
            base.Records.Add(new CurvedRecord(this.cx * 0.146, -this.cy * 0.146, this.cx * 0.206, 0.0));
            base.Records.Add(new CurvedRecord(this.cx * 0.206, 0.0, this.cx * 0.146, this.cy * 0.146));
            base.Records.Add(new CurvedRecord(this.cx * 0.146, this.cy * 0.146, 0.0, this.cy * 0.206));
            base.Records.Add(new CurvedRecord(0.0, this.cy * 0.206, -this.cx * 0.146, this.cy * 0.146));
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

