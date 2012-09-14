namespace SwfDotnet.Shapes
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Shapes;
    using SwfDotnet.Format.Tags;
    using System;

    public class LineTool : DefineShape
    {
        public LineTool()
        {
            this.LineStyles.Add(new LineStyle(new RGB(0, 0, 0), 1));
            this.FillStyles.Add(new FillStyle(new RGB(120, 120, 120)));
        }

        public void CurveTo(int ADeltaX, int ADeltaY, int BDeltaX, int BDeltaY)
        {
            base.Records.Add(new CurvedRecord(ADeltaX, ADeltaY, BDeltaX, BDeltaY));
        }

        public void HorizontalTo(int DeltaX)
        {
            base.Records.Add(new StraightRecord(false, DeltaX));
        }

        public void LineTo(int DeltaX, int DeltaY)
        {
            base.Records.Add(new StraightRecord(DeltaX, DeltaY));
        }

        protected override void OnCompile()
        {
            base.OnCompile();
        }

        public void SetStyle(int LineIndex, int StyleIndex)
        {
            base.Records.Add(new StyleChangeRecord(LineIndex, StyleIndex, 0));
        }

        public void VerticalTo(int DeltaY)
        {
            base.Records.Add(new StraightRecord(true, DeltaY));
        }

        public FillStyleArray FillStyles
        {
            get
            {
                return base.FillStyles;
            }
        }

        public LineStyleArray LineStyles
        {
            get
            {
                return base.LineStyles;
            }
        }
    }
}

