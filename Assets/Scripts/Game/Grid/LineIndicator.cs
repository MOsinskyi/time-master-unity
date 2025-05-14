using UnityEngine;

namespace Game.Grid
{
    public class LineIndicator : MonoBehaviour
    {
        public int[,] LineData { get; private set; }
        public int[] ColumnIndexes { get; private set; }

        public void Initialize(int boardSize)
        {
            LineData = new int[boardSize, boardSize];
            ColumnIndexes = new int[boardSize];
            FillLineData();
            ColumnIndexes = GetRow(0);
        }

        private (int, int) GetSquarePosition(int squareIndex)
        {
            var posRow = -1;
            var posCol = -1;

            for (var row = 0; row < LineData.GetLength(0); row++)
            for (var col = 0; col < LineData.GetLength(1); col++)
            {
                if (LineData[row, col] != squareIndex) continue;
                posRow = row;
                posCol = col;
            }

            return (posRow, posCol);
        }

        public int[] GetVerticalLine(int squareIndex)
        {
            var line = new int[LineData.GetLength(0)];

            var squarePositionCol = GetSquarePosition(squareIndex).Item2;

            for (var i = 0; i < LineData.GetLength(0); i++) line[i] = LineData[i, squarePositionCol];

            return line;
        }

        public int[] GetRow(int row)
        {
            var rowArray = new int[LineData.GetLength(0)];

            for (var i = 0; i < LineData.GetLength(0); i++) rowArray[i] = LineData[row, i];

            return rowArray;
        }

        private void FillLineData()
        {
            var index = 0;

            for (var row = 0; row < LineData.GetLength(0); row++)
            for (var col = 0; col < LineData.GetLength(1); col++)
            {
                LineData[row, col] = index;
                index++;
            }
        }
    }
}