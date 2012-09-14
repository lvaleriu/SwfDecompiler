namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class FillStyleArray : ArrayData
    {
        private IShapeData Parent;

        public FillStyleArray(IShapeData shape)
        {
            this.Parent = shape;
        }

        public override int Add(object value)
        {
            ((FillStyle) value).Parent = this.Parent;
            return base.Add(value);
        }

        protected override void OnCompile()
        {
            this.Insert(0, new UI8(base.Count));
        }
    }
}

