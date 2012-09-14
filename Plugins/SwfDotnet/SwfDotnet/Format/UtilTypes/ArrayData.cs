namespace SwfDotnet.Format.UtilTypes
{
    using SwfDotnet.Format.BasicTypes;
    using System;
    using System.Collections;

    public class ArrayData : IBytes
    {
        protected ArrayList _arr = new ArrayList();
        private bool _Compiled = false;

        public virtual int Add(object Item)
        {
            return this._arr.Add(Item);
        }

        public virtual void Compile()
        {
            if (!this._Compiled)
            {
                this._Compiled = true;
                this.OnCompile();
            }
        }

        public virtual void Insert(int Pos, object Item)
        {
            this._arr.Insert(Pos, Item);
        }

        protected virtual void OnCompile()
        {
        }

        public int Count
        {
            get
            {
                return this._arr.Count;
            }
        }

        public virtual byte[] GetBytes
        {
            get
            {
                this.Compile();
                byte[] array = new byte[this.Length];
                int index = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    IBytes bytes = (IBytes) this._arr[i];
                    bytes.GetBytes.CopyTo(array, index);
                    index += bytes.Length;
                }
                return array;
            }
        }

        public virtual int Length
        {
            get
            {
                this.Compile();
                int num = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    IBytes bytes = (IBytes) this._arr[i];
                    num += bytes.Length;
                }
                return num;
            }
        }
    }
}

