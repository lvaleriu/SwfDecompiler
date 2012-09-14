namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    internal class RecordHeader : ArrayData
    {
        public RecordHeader(int tipo, int length)
        {
            AB item = new AB(2, true);
            item.Append(new UB(10, tipo));
            if (length > 0x3e)
            {
                item.Append(new Bits("111111"));
                this.Add(item);
                this.Add(new UI32(length));
            }
            else
            {
                item.Append(new UB(6, length));
                this.Add(item);
            }
        }
    }
}

