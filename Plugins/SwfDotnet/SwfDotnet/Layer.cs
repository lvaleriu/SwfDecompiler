namespace SwfDotnet
{
    using System;
    using System.Collections;

    public class Layer
    {
        private ArrayList chars = new ArrayList();
        public string Name;

        public Layer(string name)
        {
            this.Name = name;
        }

        public void Add(int CharID)
        {
            this.chars.Add(CharID);
        }
    }
}

