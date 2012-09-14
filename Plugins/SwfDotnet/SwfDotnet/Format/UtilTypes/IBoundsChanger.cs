namespace SwfDotnet.Format.UtilTypes
{
    using System;

    public interface IBoundsChanger
    {
        void RecordToOrig(Boundarier bounds);
        void UpdateBounds(Boundarier bounds);
    }
}

