using System.Collections.Generic;
using System.Linq;
using Game.Shape;
using Sound;
using UnityEngine;

namespace Game.Grid
{
    public class LineCompletion : MonoBehaviour
    {
        private Board _board;
        private LineIndicator _lineIndicator;

        private void Start()
        {
            _lineIndicator = GetComponent<LineIndicator>();
            _board = GetComponent<Board>();
            _lineIndicator.Initialize(_board.Size);
        }

        public void CheckIfAnyLineIsCompleted()
        {
            var lines = _lineIndicator.ColumnIndexes.Select(column => _lineIndicator.GetVerticalLine(column)).ToList();

            for (var row = 0; row < _board.Size; row++)
                lines.Add(_lineIndicator.GetRow(row));

            var completedLines = CheckIfSquaresAreCompleted(lines);

            if (completedLines > 2)
            {
                //TODO: Play bonus animation
            }

            if (completedLines > 0)
            {
                GameEvents.UpdateShapeColor?.Invoke();
                SoundManager.Instance.PlaySfx(Config.AudioName.LineCompleted);
            }

            var totalScores = _board.UIScore.LineCompletedScore * completedLines;
            GameEvents.AddScore?.Invoke(totalScores);
            GetComponent<GameState>().CheckIfPlayerLost();
        }

        private int CheckIfSquaresAreCompleted(List<int[]> data)
        {
            var completedLines = new List<int[]>();

            var linesCompleted = 0;

            foreach (var line in data)
            {
                var lineCompleted = true;
                foreach (var squareIndex in line)
                {
                    var comp = _board.BoardSquares[squareIndex].GetComponent<BoardSquare>().SquareOccupied;
                    if (!comp) lineCompleted = false;
                }

                if (lineCompleted) completedLines.Add(line);
            }

            foreach (var line in completedLines)
            {
                var completed = false;

                foreach (var squareIndex in line)
                {
                    var comp = _board.BoardSquares[squareIndex].GetComponent<BoardSquare>();
                    comp.Deactivate();
                    completed = true;
                }

                foreach (var squareIndex in line)
                {
                    var comp = _board.BoardSquares[squareIndex].GetComponent<BoardSquare>();
                    comp.ClearOccupied();
                }

                if (completed) linesCompleted++;
            }

            return linesCompleted;
        }
    }
}