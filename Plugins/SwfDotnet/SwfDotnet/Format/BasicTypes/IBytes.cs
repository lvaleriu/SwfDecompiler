namespace SwfDotnet.Format.BasicTypes
{
    using System;

    public interface IBytes
    {
        byte[] GetBytes { get; }

        int Length { get; }
    }
}

