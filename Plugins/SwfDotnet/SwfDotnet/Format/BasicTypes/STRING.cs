namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct STRING : IBytes
    {
        private string _val;
        private bool EOS;
        public STRING(string val)
        {
            this._val = val;
            this.EOS = true;
        }

        public STRING(string val, bool EndOfString)
        {
            this._val = val;
            this.EOS = EndOfString;
        }

        public byte[] GetBytes
        {
            get
            {
                int length = this._val.Length;
                if (this.EOS)
                {
                    length++;
                }
                byte[] buffer = new byte[length];
                for (int i = 0; i < this._val.Length; i++)
                {
                    byte num3 = Convert.ToByte(Convert.ToChar(this._val.Substring(i, 1)));
                    buffer[i] = BitConverter.GetBytes((short) num3)[0];
                }
                if (this.EOS)
                {
                    buffer[length - 1] = Convert.ToByte(0);
                }
                return buffer;
            }
        }
        public int Length
        {
            get
            {
                int length = this._val.Length;
                if (this.EOS)
                {
                    length++;
                }
                return length;
            }
        }
    }
}

