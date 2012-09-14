namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class EndShapeRecord : AB
    {
        public EndShapeRecord() : base(1)
        {
            this.Append(new UB(1, 0));
            this.Append(new UB(5, 0));
        }
    }
}

