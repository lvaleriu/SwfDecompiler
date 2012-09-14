namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class BaseTag : ArrayData
    {
        private RecordHeader _TagHeader;
        private int _TagID;

        public BaseTag(int tagid)
        {
            this._TagID = tagid;
        }

        protected override void OnCompile()
        {
            this._TagHeader = new RecordHeader(this._TagID, this.Length);
            this.Insert(0, this._TagHeader);
            base.OnCompile();
        }
    }
}

