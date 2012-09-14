namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class LineStyle : ArrayData
    {
        public RGB Color;
        public int Width = 0;

        public LineStyle(RGB color, int width)
        {
            this.Width = width;
            this.Color = color;
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(this.Width));
            this.Add(this.Color);
        }
    }
}

