using System;
using System.Collections.Generic;
using System.Text;

namespace PSM_PD4.Models
{
    public struct InsideTemperature
    {
        public int i;
        public int j;
        public string Name;
        public int Value;

        public InsideTemperature(int i, int j, string name, int Value = 0)
        {
            this.i = i;
            this.j = j;
            this.Name = name;
            this.Value = Value;
        }

        public void Deconstruct(out int i, out int j) =>
       (i,j) = (this.i, this.j);

        public override bool Equals(object obj) => obj is InsideTemperature insideTemp
            && insideTemp.i == this.i
            && insideTemp.j == this.j;

    }
}
