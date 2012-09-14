namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Shapes;
    using System;

    public class TagDefineShape : Character
    {
        private ShapeWithStyle Shape;

        public TagDefineShape(ShapeWithStyle shape) : base(2)
        {
            this.Shape = shape;
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.CharacterID));
            this.Add(this.Shape.Bounds);
            this.Add(this.Shape);
            base.OnCompile();
        }
    }
}

