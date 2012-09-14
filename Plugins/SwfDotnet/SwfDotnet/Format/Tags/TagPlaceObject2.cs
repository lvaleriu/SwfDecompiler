namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class TagPlaceObject2 : BaseTag, IDepthChanger
    {
        private Character _character;
        private int _deph;
        private Matrix _matrix;
        public CXFORM ColorTransform;
        public string Name;
        public int Ratio;

        public TagPlaceObject2(Character character, int Deph, Matrix TransformMatrix) : base(0x1a)
        {
            this._deph = 1;
            this._matrix = TransformMatrix;
            this._character = character;
            this._deph = Deph;
        }

        protected override void OnCompile()
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            if (this._matrix != null)
            {
                flag = true;
            }
            if (this._character != null)
            {
                flag3 = true;
            }
            if (this.Name != null)
            {
                flag2 = true;
            }
            if (this.ColorTransform != null)
            {
                flag4 = true;
            }
            AB item = new AB(1);
            item.Append(new UB(2, 0));
            item.Append(new UB(1, Convert.ToInt32( flag2)));
            item.Append(new UB(1, Convert.ToInt32( flag5)));
            item.Append(new UB(1, Convert.ToInt32( flag4)));
            item.Append(new UB(1, Convert.ToInt32( flag)));
            item.Append(new UB(1, Convert.ToInt32( flag3)));
            item.Append(new UB(1, 0));
            this.Add(item);
            this.Add(new UI16(this._character.Depth));
            if (flag3)
            {
                this.Add(new UI16(this._character.CharacterID));
            }
            if (flag)
            {
                this.Add(this._matrix);
            }
            if (flag4)
            {
                this.Add(this.ColorTransform);
            }
            if (flag5)
            {
                this.Add(new UI16(this.Ratio));
            }
            if (flag2)
            {
                this.Add(new STRING(this.Name));
            }
            base.OnCompile();
        }

        public int SetDepth(int depth)
        {
            this._character.Depth = depth;
            return 1;
        }
    }
}

