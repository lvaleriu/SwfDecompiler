namespace SwfDotnet.Format.Shapes
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class RecordArray : AB
    {
        private IShapeData Parent;

        public RecordArray(IShapeData parent) : base(-1)
        {
            this.Parent = parent;
        }

        public void Add(object record)
        {
            if (record is StyleChangeRecord)
            {
                ((StyleChangeRecord) record).Parent = this.Parent;
            }
            base.IBitables.Add((IBitable) record);
        }

        public void Insert(int Index, object record)
        {
            if (record is StyleChangeRecord)
            {
                ((StyleChangeRecord) record).Parent = this.Parent;
            }
            base.IBitables.Insert(Index, (IBitable) record);
        }
    }
}

