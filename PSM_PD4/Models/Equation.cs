using System;
using System.Collections.Generic;
using System.Text;

namespace PSM_PD4.Models
{
    public struct Equation
    {
        public string X;
        public int result;
        public int[] values;

        public Equation(string x, int result, int[] equation)
        {
            X = x;
            this.result = result;
            this.values = equation;
        }
    }
}
