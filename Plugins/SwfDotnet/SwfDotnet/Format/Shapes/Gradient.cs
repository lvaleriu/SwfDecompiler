namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class Gradient : ArrayData
    {
        public int OffsetX;
        public int OffsetY;
        public int PercentScaleX;
        public int PercentScaleY;
        public int Rotation;
        public GradientType Type;

        public Gradient(GradientType type)
        {
            this.OffsetX = 0;
            this.OffsetY = 0;
            this.Rotation = 0;
            this.Type = GradientType.LinearGradient;
            this.PercentScaleX = 100;
            this.PercentScaleY = 100;
            this.Type = type;
        }

        public Gradient(GradientType type, int Angle) : this(type)
        {
            this.Rotation = Angle;
        }

        public Gradient(GradientType type, int offsetX, int offsetY) : this(type)
        {
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
        }

        public void AddColor(RGB Color, int Position)
        {
            if (Position > 0xff)
            {
                Position = 0xff;
            }
            if (Position < 0)
            {
                Position = 0;
            }
            if (base.Count <= 7)
            {
                this.Add(new GradRecord(Position, Color));
            }
        }

        public void MoveOffset(int offsetX, int offsetY)
        {
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
        }

        protected override void OnCompile()
        {
            this.Insert(0, new UI8(base.Count));
        }

        public void Scale(int PercentX, int PercentY)
        {
            this.PercentScaleX = PercentX;
            this.PercentScaleY = PercentY;
        }
    }
}

