namespace SwfDotnet
{
    using SwfDotnet.Format.UtilTypes;
    using System;
    using System.Reflection;

    public class FrameArray : ArrayData
    {
        public Frame this[int index]
        {
            get
            {
                if (index <= 0)
                {
                    throw new Exception("FrameArray is 1 based array. First Frame has index 0");
                }
                int num = index - base._arr.Count;
                for (int i = 0; i < num; i++)
                {
                    this.Add(new Frame());
                }
                return (Frame) base._arr[index - 1];
            }
        }
    }
}

