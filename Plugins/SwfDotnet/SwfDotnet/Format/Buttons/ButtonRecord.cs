namespace SwfDotnet.Format.Buttons
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    internal class ButtonRecord : ArrayData
    {
        public ButtonRecord(int CharacterID, bool hitTest, bool Down, bool Over, bool Up)
        {
            AB item = new AB(1);
            item.Append(new UB(4, 0));
            item.Append(new UB(1, Convert.ToInt32( hitTest)));
            item.Append(new UB(1, Convert.ToInt32( Down)));
            item.Append(new UB(1, Convert.ToInt32( Over)));
            item.Append(new UB(1, Convert.ToInt32( Up)));
            this.Add(item);
            this.Add(new UI16(CharacterID));
            this.Add(new UI16(1));
            Matrix matrix = new Matrix();
            matrix.Tanslate(200, 200);
            this.Add(matrix);
        }
    }
}

