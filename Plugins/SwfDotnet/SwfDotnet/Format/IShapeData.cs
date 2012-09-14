namespace SwfDotnet.Format
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public interface IShapeData
    {
        RECT Bounds { get; }

        int NumFillBits { get; }

        int NumLineBits { get; }
    }
}

