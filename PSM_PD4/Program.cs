using System;
using System.Drawing;
using System.Linq;

namespace PSM_PD4
{
    class Program
    {
        static void Main(string[] args)
        {           
            Tile tile = new Tile(new Size(42, 42), 100, 50, 200, 150); //42x42 = 1600 unknowns         
            var equations = tile.CalucalateTemperatures();

            var results = equations
                            .ToList()
                            .Select(eq => eq.result)
                            .ToArray();

            var values = equations
                           .ToList()
                           .Select(eq => eq.values)
                           .ToArray();
           
            Console.WriteLine(Gauss.GaussElimination.SolveEquations(values,results));

        }
    }
}
