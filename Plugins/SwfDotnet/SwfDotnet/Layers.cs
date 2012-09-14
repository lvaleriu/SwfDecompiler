namespace SwfDotnet
{
    using System;
    using System.Collections;

    public class Layers
    {
        private Layer Current;
        private ArrayList lys = new ArrayList();

        public void Add(string Name)
        {
            Layer layer = new Layer(Name);
            this.lys.Add(layer);
            this.Current = layer;
        }

        public void AdjustDephs()
        {
            for (int i = 0; i < this.lys.Count; i++)
            {
            }
        }
    }
}

