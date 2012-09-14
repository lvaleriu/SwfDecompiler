namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Collections;
    using System.Text;

    public class AB : IBytes, IBitable
    {
        private bool _Compiled;
        private int _nBytes;
        private bool _reverse;
        private StringBuilder _temp;
        public ArrayList IBitables;

        public AB(int numBytes)
        {
            this.IBitables = new ArrayList();
            this._Compiled = false;
            this._nBytes = numBytes;
            this._temp = new StringBuilder();
            this._reverse = false;
        }

        public AB(int numBytes, bool reverse)
        {
            this.IBitables = new ArrayList();
            this._Compiled = false;
            this._nBytes = numBytes;
            this._temp = new StringBuilder();
            this._reverse = reverse;
        }

        public virtual void Append(IBitable bitable)
        {
            this.IBitables.Add(bitable);
        }

        public virtual void Compile()
        {
            if (!this._Compiled)
            {
                this._Compiled = true;
                this.OnCompile();
            }
        }

        private void Complete()
        {
            this._nBytes = this.Length;
            int length = this._temp.Length;
            if (length > (this._nBytes * 8))
            {
                throw new Exception("AB Overflow");
            }
            this._temp.Append(new string('0', (this._nBytes * 8) - length));
        }

        public string GetBits()
        {
            this.Compile();
            this.MakeString();
            return this._temp.ToString();
        }

        private void MakeString()
        {
            if (this._temp.Length <= 0)
            {
                for (int i = 0; i < this.IBitables.Count; i++)
                {
                    this._temp.Append(((IBitable) this.IBitables[i]).GetBits());
                }
            }
        }

        protected virtual void OnCompile()
        {
        }

        public byte[] GetBytes
        {
            get
            {
                this.Compile();
                this.MakeString();
                this.Complete();
                byte[] buffer = new byte[this._nBytes];
                for (int i = 0; i < this._nBytes; i++)
                {
                    if (!this._reverse)
                    {
                        buffer[i] = Convert.ToByte(this._temp.ToString().Substring(i * 8, 8), 2);
                    }
                    else
                    {
                        buffer[this._nBytes - (i + 1)] = Convert.ToByte(this._temp.ToString().Substring(i * 8, 8), 2);
                    }
                }
                return buffer;
            }
        }

        public int Length
        {
            get
            {
                this.Compile();
                this.MakeString();
                if (this._nBytes == -1)
                {
                    return (int) Math.Ceiling((double) (((float) this._temp.Length) / 8f));
                }
                return this._nBytes;
            }
        }
    }
}

