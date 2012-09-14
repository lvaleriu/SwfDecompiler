namespace SwfDotnet.Format.ActionScript
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;
    using System.Collections;

    public class Script : ArrayData
    {
        public void And()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x10);
            this.Add(item);
        }

        public void DeclareDictionary(string[] data)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x88);
            item.Add(new UI16(data.Length));
            for (int i = 0; i < data.Length; i++)
            {
                item.Add(new STRING(data[i], true));
            }
            this.Add(item);
        }

        public void DefineVar()
        {
            this.Add(new SwfDotnet.Format.ActionScript.Action(0x41));
        }

        public void Else()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x99);
            item.Jump = true;
            this.Add(item);
        }

        public void EndIf()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0);
            this.Add(item);
        }

        public void Equal()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(14);
            this.Add(item);
        }

        public void GetVar()
        {
            this.Add(new SwfDotnet.Format.ActionScript.Action(0x1c));
        }

        public Script GetVar(string varName)
        {
            Script script = new Script();
            script.Push(varName);
            script.GetVar();
            return script;
        }

        public void gotoAndPlay(int FrameNumber)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x81);
            item.Add(new UI16(FrameNumber - 1));
            this.Add(item);
            this.Play();
        }

        public void gotoAndStop(int FrameNumber)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x81);
            item.Add(new UI16(FrameNumber - 1));
            this.Add(item);
        }

        public void If()
        {
            this.Not();
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x9d);
            item.Jump = true;
            this.Add(item);
        }

        public void Inc()
        {
            this.Add(new SwfDotnet.Format.ActionScript.Action(80));
        }

        public void LessThan()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(15);
            this.Add(item);
        }

        public void Not()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x12);
            this.Add(item);
        }

        protected override void OnCompile()
        {
            SwfDotnet.Format.ActionScript.Action action = null;
            int num = 0;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            ArrayList list = (ArrayList) base._arr.Clone();
            foreach (SwfDotnet.Format.ActionScript.Action action2 in list)
            {
                if (action2._code == 0x9d)
                {
                    flag = true;
                    action = action2;
                }
                else
                {
                    if (action2._code == 0x99)
                    {
                        flag3 = true;
                    }
                    if (action2._code == 0)
                    {
                        flag2 = true;
                        base._arr.Remove(action2);
                    }
                    if (flag2)
                    {
                        action.JumpOffset = num;
                        num = 0;
                        action = null;
                        flag2 = false;
                        flag = false;
                    }
                    if (flag3)
                    {
                        flag3 = false;
                        action.JumpOffset = num + 5;
                        num = 0;
                        action = action2;
                        continue;
                    }
                    if (flag)
                    {
                        num += action2.Length;
                    }
                }
            }
            base.OnCompile();
        }

        public void Or()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x11);
            this.Add(item);
        }

        public void Play()
        {
            this.Add(new SwfDotnet.Format.ActionScript.Action(6));
        }

        public void Pop()
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x17);
            this.Add(item);
        }

        public void Push(int DicIndex)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(150);
            item.Add(new UI8(5));
            item.Add(new UI8(DicIndex));
            this.Add(item);
        }

        public void Push(string Data)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(150);
            item.Add(new UI8(0));
            item.Add(new STRING(Data));
            this.Add(item);
        }

        public void Push2(int Data, int Data2)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(150);
            item.Add(new UI8(8));
            item.Add(new UI8(Data));
            item.Add(new UI8(8));
            item.Add(new UI8(Data2));
            this.Add(item);
        }

        public void Push2(string Data, string Data2)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(150);
            item.Add(new UI8(0));
            item.Add(new STRING(Data));
            item.Add(new UI8(0));
            item.Add(new STRING(Data2));
            this.Add(item);
        }

        public void Push3(string Data, string Data2, string Data3)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(150);
            item.Add(new UI8(0));
            item.Add(new STRING(Data));
            item.Add(new UI8(0));
            item.Add(new STRING(Data2));
            item.Add(new UI8(0));
            item.Add(new STRING(Data3));
            this.Add(item);
        }

        public void SetVar()
        {
            this.Add(new SwfDotnet.Format.ActionScript.Action(0x1d));
        }

        public void SetVar(string varName, Script RightVal)
        {
            this.Push(varName);
            base._arr.AddRange(RightVal._arr);
            this.Add(new SwfDotnet.Format.ActionScript.Action(0x1d));
        }

        public void Stop()
        {
            this.Add(new SwfDotnet.Format.ActionScript.Action(7));
        }

        public Script Sum(Script A, Script B)
        {
            Script script = new Script();
            script._arr.AddRange(A._arr);
            script._arr.AddRange(B._arr);
            script.Add(new SwfDotnet.Format.ActionScript.Action(10));
            return script;
        }

        public void TellTarget(string Name)
        {
            SwfDotnet.Format.ActionScript.Action item = new SwfDotnet.Format.ActionScript.Action(0x8b);
            item.Add(new STRING(Name));
            this.Add(item);
        }
    }
}

