namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Shapes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class DefineShape : Character, IShapeData
    {
        protected RECT _bounds;
        protected FillStyleArray FillStyles;
        private bool hasBounds;
        protected LineStyleArray LineStyles;
        protected int nFillBits;
        protected int nLineBits;
        public RecordArray Records;

        public DefineShape() : base(2)
        {
            this.LineStyles = new LineStyleArray();
            this.nFillBits = 0;
            this.nLineBits = 0;
            this.hasBounds = false;
            this.Records = new RecordArray(this);
            this.FillStyles = new FillStyleArray(this);
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.CharacterID));
            this.Add(this.Bounds);
            this.Add(this.FillStyles);
            this.Add(this.LineStyles);
            AB item = new AB(1);
            if (this.FillStyles.Count > 0)
            {
                this.nFillBits = UB.NumBits((long) this.FillStyles.Count);
            }
            if (this.LineStyles.Count > 0)
            {
                this.nLineBits = UB.NumBits((long) this.LineStyles.Count);
            }
            item.Append(new UB(4, this.nFillBits));
            item.Append(new UB(4, this.nLineBits));
            this.Add(item);
            this.Records.Add(new EndShapeRecord());
            this.Add(this.Records);
            base.OnCompile();
        }

        public RECT Bounds
        {
            get
            {
                if (!this.hasBounds)
                {
                    this.Compile();
                    Boundarier bounds = new Boundarier();
                    for (int i = 0; i < this.Records.IBitables.Count; i++)
                    {
                        if (this.Records.IBitables[i] is IBoundsChanger)
                        {
                            ((IBoundsChanger) this.Records.IBitables[i]).UpdateBounds(bounds);
                        }
                    }
                    bounds.OpenRect((int) (((double) this.LineStyles.MaxLineWidth) / 2.0));
                    bounds.MoveToOrig();
                    for (int j = 0; j < this.Records.IBitables.Count; j++)
                    {
                        if (this.Records.IBitables[j] is IBoundsChanger)
                        {
                            ((IBoundsChanger) this.Records.IBitables[j]).RecordToOrig(bounds);
                        }
                    }
                    this._bounds = bounds.Bounds;
                    this.hasBounds = true;
                }
                return this._bounds;
            }
        }

        public int NumFillBits
        {
            get
            {
                return this.nFillBits;
            }
        }

        public int NumLineBits
        {
            get
            {
                return this.nLineBits;
            }
        }
    }
}

