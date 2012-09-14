namespace SwfDotnet
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    internal class Header : ArrayData
    {
        private int _FileLength;
        private UI16 _FrameCount;
        private FB8 _FrameRate;
        private RECT _FrameSize;
        private UI8 _version = new UI8(6);

        public Header(int width, int height, int fileLength, int FrameCount, int FrameRate)
        {
            this._FileLength = fileLength;
            this._FrameSize = new RECT(0, 0, width, height);
            this._FrameCount = new UI16(FrameCount);
            this._FrameRate = new FB8((float) FrameRate);
        }

        protected override void OnCompile()
        {
            this.Add(new UI8('F'));
            this.Add(new UI8('W'));
            this.Add(new UI8('S'));
            this.Add(this._version);
            this.Add(new UI32(this._FileLength + (12 + this._FrameSize.Length)));
            this.Add(this._FrameSize);
            this.Add(this._FrameRate);
            this.Add(this._FrameCount);
        }
    }
}

