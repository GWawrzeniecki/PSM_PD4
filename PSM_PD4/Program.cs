using System;
using System.Drawing;

namespace PSM_PD4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Tile tile = new Tile(new Size(42, 42), 100, 50, 200, 150);
            tile.CalucalateTemperatures();
        }
    }
}
