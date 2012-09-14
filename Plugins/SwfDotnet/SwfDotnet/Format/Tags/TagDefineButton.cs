namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.ActionScript;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Buttons;
    using System;

    public class TagDefineButton : Character
    {
        private Character _Down;
        private Character _Hit;
        private Character _Over;
        private Character _Up;
        public SwfDotnet.Format.ActionScript.Script Script;

        public TagDefineButton() : base(7)
        {
            this.Script = new SwfDotnet.Format.ActionScript.Script();
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.CharacterID));
            if (this._Hit != null)
            {
                this.Add(new ButtonRecord(this._Hit.CharacterID, true, false, false, false));
            }
            if (this._Down != null)
            {
                this.Add(new ButtonRecord(this._Down.CharacterID, false, true, false, false));
            }
            if (this._Over != null)
            {
                this.Add(new ButtonRecord(this._Over.CharacterID, false, false, true, false));
            }
            if (this._Up != null)
            {
                this.Add(new ButtonRecord(this._Up.CharacterID, false, false, false, true));
            }
            this.Add(new UI8(0));
            SwfDotnet.Format.ActionScript.Script script = new SwfDotnet.Format.ActionScript.Script();
            script.DeclareDictionary(new string[] { "Variable", "2", "Z" });
            script.SetVar("P", script.GetVar("Z"));
            script.SetVar("Z", script.Sum(script.GetVar("P"), script.GetVar("lo")));
            if (this.Script.Count > 0)
            {
                this.Add(this.Script);
                this.Add(new UI8(0));
            }
            base.OnCompile();
        }

        public Character Down
        {
            get
            {
                return this._Down;
            }
            set
            {
                this._Down = value;
            }
        }

        public Character Hit
        {
            get
            {
                return this._Hit;
            }
            set
            {
                this._Hit = value;
            }
        }

        public Character Over
        {
            get
            {
                return this._Over;
            }
            set
            {
                this._Over = value;
            }
        }

        public Character Up
        {
            get
            {
                return this._Up;
            }
            set
            {
                this._Up = value;
            }
        }
    }
}

