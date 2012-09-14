namespace SwfDotnet
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.ActionScript;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Tags;
    using SwfDotnet.Format.UtilTypes;
    using System;

    public class Frame : ArrayData
    {
        public SwfDotnet.Format.ActionScript.Script Script = new SwfDotnet.Format.ActionScript.Script();

        public void Add(Character Character)
        {
            base.Add(new TagPlaceObject2(Character, 1, null));
        }

        public void Add(Character Character, Matrix TransformMatrix)
        {
            base.Add(new TagPlaceObject2(Character, 1, TransformMatrix));
        }

        public void Add(Character Character, string Name)
        {
            this.Add(Character, Name, null);
        }

        public void Add(Character Character, string Name, Matrix TransformMatrix)
        {
            TagPlaceObject2 item = new TagPlaceObject2(Character, 1, TransformMatrix);
            item.Name = Name;
            base.Add(item);
        }

        public int AdjustDephts(int Depth)
        {
            int num = 0;
            for (int i = 0; i < base._arr.Count; i++)
            {
                if (base._arr[i] is IDepthChanger)
                {
                    num += ((IDepthChanger) base._arr[i]).SetDepth(num + Depth);
                }
            }
            return num;
        }

        protected override void OnCompile()
        {
            if (this.Script.Count > 0)
            {
                this.Add(new TagDoAction(this.Script));
            }
            this.Add(new TagShowFrame());
        }

        public void Remove(Character Character)
        {
            base.Add(new TagRemoveObject2(Character));
        }
    }
}

