using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using PSM_PD4.Models;

namespace PSM_PD4
{
    class Tile
    {

        public Size TileSize { get; }
        public int LeftSideTemperature { get; }
        public int RightSideTemperature { get; }
        public int UpsideTemperature { get; }
        public int DownsideTemperature { get; }

        private int[] _leftSideEdgeTemperatures;
        private int[] _rightSideEdgeTemperatures;
        private int[] _upsideEdgeTemperatures;
        private int[] _downsideEdgeTemperatures;
        private InsideTemperature[][] _insideTempertatures;
        private Equation[] _equations;
        private int GetUnknownTemperaturesAmount => (TileSize.Width * TileSize.Height) - (2 * TileSize.Height + 2 * TileSize.Width);
        //public string PatternSeparator = ";";
        //public string Pattern => $"Ti+1,j{PatternSeparator}-4Ti,j{PatternSeparator}+Ti-1,j{PatternSeparator}+Ti,j+1{PatternSeparator}+Ti,j-1{PatternSeparator}0";


        public Tile(Size tileSize, int leftSideTemperature, int rightSideTemperature, int upsideTemperature, int downsideTemperature)
        {
            TileSize = tileSize;
            LeftSideTemperature = leftSideTemperature;
            RightSideTemperature = rightSideTemperature;
            UpsideTemperature = upsideTemperature;
            DownsideTemperature = downsideTemperature;
            _equations = new Equation[GetUnknownTemperaturesAmount];
            BuildTile();
        }

        private void BuildTile()
        {
            BuildEdgeTemperatures();
            BuildInsideTemperatures();
        }

        private void BuildEdgeTemperatures()
        {
            _leftSideEdgeTemperatures = new int[TileSize.Height];
            _rightSideEdgeTemperatures = new int[TileSize.Height];
            _upsideEdgeTemperatures = new int[TileSize.Width];
            _downsideEdgeTemperatures = new int[TileSize.Height];

            FillArray(ref _leftSideEdgeTemperatures, LeftSideTemperature);
            FillArray(ref _rightSideEdgeTemperatures, RightSideTemperature);
            FillArray(ref _upsideEdgeTemperatures, UpsideTemperature);
            FillArray(ref _downsideEdgeTemperatures, DownsideTemperature);
        }

        private void FillArray<T>(ref T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
        }

        private void BuildInsideTemperatures()
        {
            _insideTempertatures = new InsideTemperature[TileSize.Height][];

            for (int h = 0; h < TileSize.Height; h++)
            {
                _insideTempertatures[h] = new InsideTemperature[TileSize.Width];
                for (int w = 0; w < TileSize.Width; w++)
                {
                    _insideTempertatures[h][w] = new InsideTemperature(w, h, $"T{w},{h}");
                }
            }
        }

        public void CalucalateTemperatures()
        {
            BuildEquations();
        }

        private void BuildEquations()
        {
            var allTemperatures = _insideTempertatures
                .SelectMany(item => item)
                .Where(t => !IsEdgeTemperature(t)) // to do moze jakos optymalniej
                .ToArray();

            int equationCounter = 0;

            for (int i = 0; i < _insideTempertatures.Length; i++)
            {
                for (int j = 1; j < _insideTempertatures[j].Length - 1; j++)
                {
                    BuildEquation(_insideTempertatures[i][j].i, _insideTempertatures[i][j].i, allTemperatures, equationCounter);
                    equationCounter++;
                }
            }
        }


        private void BuildEquation(int i, int j, InsideTemperature[] allTemperatures, int equationCounter)
        {
            var valuesFromPattern = FillThePattern(i, j);
            var edgeTemperatures = FindEdgesTemperatures(valuesFromPattern);
            var nonEdgeTemperatures = valuesFromPattern.Except(edgeTemperatures)
                .ToArray();

            var values = new List<int>();

            var restUnknownTemperatures = allTemperatures
                .Except(nonEdgeTemperatures);

     
           



        }


        private InsideTemperature[] FindEdgesTemperatures(InsideTemperature[] valuesFromPattern)
        {
            return valuesFromPattern
                .Where(t => IsEdgeTemperature(t))
                .Select(t => GetEdgeTemperature(t))
                .ToArray();
        }

        private bool IsEdgeTemperature(InsideTemperature insideTemperature)
        {
            return insideTemperature.i == 0 || insideTemperature.j == 0 || insideTemperature.i == TileSize.Width - 1 || insideTemperature.j == TileSize.Height - 1;
        }

        private InsideTemperature GetEdgeTemperature(InsideTemperature insideTemperature)
        {
            if (!IsEdgeTemperature(insideTemperature))
                throw new ArgumentException($"This temperature isn't a edge temperature i:{insideTemperature.i} j:{insideTemperature.j}");

            insideTemperature.Value = ComputeSalesTax(insideTemperature);
            return insideTemperature;
        }

        private int ComputeSalesTax(InsideTemperature insideTemperature) =>
    insideTemperature switch
    {
        { i: 0 } => _leftSideEdgeTemperatures[0],
        { j: 41 } => _upsideEdgeTemperatures[0],
        { i: 41 } => _rightSideEdgeTemperatures[0],
        { j: 0 } => _downsideEdgeTemperatures[0],
        // other cases removed for brevity...
        _ => 0
    };

        private void test(int i, int j)
        {

        }

        private InsideTemperature[] FillThePattern(int i, int j)
        {
            var result = new InsideTemperature[5];
            result[0] = new InsideTemperature(i + 1, j, $"T{i + 1},{j}", 1);
            result[1] = new InsideTemperature(i, j, $"T{i},{j}", -4);
            result[2] = new InsideTemperature(i - 1, j, $"T{i - 1},{j}", 1);
            result[3] = new InsideTemperature(i, j + 1, $"T{i},{j + 1}", 1);
            result[4] = new InsideTemperature(i, j - 1, $"T{i},{j - 1}", 1);
            return result;
        }
    }
}
