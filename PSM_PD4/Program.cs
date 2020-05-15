using System;
using System.Drawing;
using System.Linq;

namespace PSM_PD4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Tile tile = new Tile(new Size(42, 42), 100, 50, 200, 150);          
            var equations = tile.CalucalateTemperatures();

            //zbic wyniki do results i zrobic [][] z rownaniami
            var values = new int[3][];

            var var1 = new int[] { 1, -3, 1 };

            var var2 = new int[] { 2, -8, 8 };

            var var3 = new int[] { -6, 3, -15 };
            values[0] = var1;
            values[1] = var2;
            values[2] = var3;
            var results = new int[] {4,-2,9 };

            var result = Gauss.GaussElimination.LoadArray(out int num, out int num_cols, values, results);
            Console.WriteLine(Gauss.GaussElimination.cmdSolve_Click(values,results));

        }
    }
}
