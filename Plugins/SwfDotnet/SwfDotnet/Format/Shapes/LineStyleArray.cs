namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class LineStyleArray : ArrayData
    {
        private int _MaxLineWidth = 0;

        public override int Add(object item)
        {
            if (item is LineStyle)
            {
                int width = ((LineStyle) item).Width;
                if (width > this._MaxLineWidth)
                {
                    this._MaxLineWidth = width;
                }
                return base.Add(item);
            }
            return -1;
        }

        protected override void OnCompile()
        {
            this.Insert(0, new UI8(base.Count));
        }

        public int MaxLineWidth
        {
            get
            {
                return this._MaxLineWidth;
            }
        }
    }
}

