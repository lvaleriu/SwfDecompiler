namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class FillStyle : ArrayData
    {
        private Gradient _Gradient;
        private Character _ImageFill;
        private RGB _SolidColor;
        private FillStyleType _Type;
        public IShapeData Parent;

        public FillStyle(RGB SolidFillColor)
        {
            this._SolidColor = SolidFillColor;
            this._Type = FillStyleType.Solid;
        }

        public FillStyle(Gradient FillGradient)
        {
            this._Gradient = FillGradient;
            this._Type = (FillStyleType) FillGradient.Type;
        }

        public FillStyle(Character ImageFill, BitmapFill bitmapFill)
        {
            this._ImageFill = ImageFill;
            this._Type = (FillStyleType) bitmapFill;
        }

        protected override void OnCompile()
        {
            this.Add(new UI8((int) this._Type));
            if (this._Type == FillStyleType.Solid)
            {
                this.Add(this._SolidColor);
            }
            if ((this._Type == FillStyleType.LinearGradient) || (this._Type == FillStyleType.RadialGradient))
            {
                Matrix item = new Matrix();
                double scaleX = 1.0 / (32768.0 / ((double) this.Parent.Bounds.Width));
                double scaleY = 1.0 / (32768.0 / ((double) this.Parent.Bounds.Height));
                scaleX *= ((double) this._Gradient.PercentScaleX) / 100.0;
                scaleY *= ((double) this._Gradient.PercentScaleY) / 100.0;
                item.Scale(scaleX, scaleY);
                if (this._Gradient.Rotation != 0)
                {
                    item.Rotate(this._Gradient.Rotation);
                }
                item.Tanslate((this.Parent.Bounds.Width / 2) + this._Gradient.OffsetX, (this.Parent.Bounds.Height / 2) + this._Gradient.OffsetY);
                this.Add(item);
                this.Add(this._Gradient);
            }
            if ((this._Type == FillStyleType.TiledBitmap) || (this._Type == FillStyleType.ClippedBitmap))
            {
                Matrix matrix2 = new Matrix();
                matrix2.Scale(20.0, 20.0);
                this.Add(new UI16(this._ImageFill.CharacterID));
                this.Add(matrix2);
            }
        }
    }
}

