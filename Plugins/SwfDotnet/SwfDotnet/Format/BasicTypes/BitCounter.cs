namespace SwfDotnet.Format.BasicTypes
{
    using System;

    public class BitCounter
    {
        private static int _maxCount = 0;

        public static void Init()
        {
            _maxCount = 0;
        }

        public static void Push(int num)
        {
            if (num > _maxCount)
            {
                _maxCount = num;
            }
        }

        public static int Maxim
        {
            get
            {
                return _maxCount;
            }
            set
            {
                _maxCount = value;
            }
        }
    }
}

