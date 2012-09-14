namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    internal class GradRecord : ArrayData
    {
        public GradRecord(int Ratio, RGB Color)
        {
            this.Add(new UI8(Ratio));
            this.Add(Color);
        }
    }
}

