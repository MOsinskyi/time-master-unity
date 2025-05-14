using System.Collections.Generic;
using System.Linq;
using Game.Shape;
using Sound;
using UnityEngine;

namespace Game.Grid
{
    public class ShapePlacement : MonoBehaviour
    {
        private const int MaxCombinationCount = 100;

        private Board _board;
        private ShapeStorage _shapeStorage;

        private void Start()
        {
            _shapeStorage = FindAnyObjectByType<ShapeStorage>();
            _board = GetComponent<Board>();
        }

        private void OnEnable()
        {
            GameEvents.CheckIfShapeCanBePlaced += PlaceShape;
        }

        private void OnDisable()
        {
            GameEvents.CheckIfShapeCanBePlaced -= PlaceShape;
        }

        private void PlaceShape()
        {
            var currentActiveShapeColor = _shapeStorage.GetCurrentSelectedShape().GetComponent<ShapeColor>()
                .GetCurrentActiveSquareColor();
            if (CheckIfShapeCanBePlaced(out var squareNumber, out var squareIndexes))
            {
                foreach (var squareIndex in squareIndexes)
                    _board.BoardSquares[squareIndex].GetComponent<BoardSquare>()
                        .PlaceShapeOnBoard(currentActiveShapeColor);

                if (_shapeStorage.ShapesLeft() == 0)
                    GameEvents.RequestNewShapes?.Invoke();
                else
                    GameEvents.SetShapeInactive?.Invoke();

                SoundManager.Instance.PlaySfx(Config.AudioName.PutDown);
                var totalScores = _board.UIScore.SquarePlacedScore * squareNumber;
                GameEvents.AddScore?.Invoke(totalScores);

                GetComponent<LineCompletion>().CheckIfAnyLineIsCompleted();
            }
            else
            {
                GameEvents.MoveShapeToStartPosition?.Invoke();
            }
        }

        public bool CheckIfShapeCanBePlaced(out int squareNumber, out List<int> squareIndexes)
        {
            squareIndexes = new List<int>();

            foreach (var boardSquare in _board.BoardSquares.Select(square => square.GetComponent<BoardSquare>())
                         .Where(boardSquare => boardSquare.Selected && !boardSquare.SquareOccupied))
            {
                squareIndexes.Add(boardSquare.SquareIndex);
                boardSquare.Selected = false;
            }

            var currentSelectedShape = _shapeStorage.GetCurrentSelectedShape();
            squareNumber = currentSelectedShape.TotalSquareNumber;
            if (currentSelectedShape == null) return false;

            return currentSelectedShape.TotalSquareNumber == squareIndexes.Count;
        }

        public bool CheckIfShapeCanBePlacedOnGrid(ShapeRenderer currentShape)
        {
            var currentShapeData = currentShape.CurrenShapeData;
            var shapeColumns = currentShapeData.Columns;
            var shapeRows = currentShapeData.Rows;

            var originalShapeFilledUpSquares = new List<int>();
            var squareIndex = 0;

            for (var row = 0; row < shapeRows; row++)
            for (var col = 0; col < shapeColumns; col++)
            {
                if (currentShapeData.Board[row].Column[col]) originalShapeFilledUpSquares.Add(squareIndex);

                squareIndex++;
            }

            if (currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
                Debug.LogError("Number of filled up squares are not the same as the original shape have");

            var squareList = GetAllSquaresCombinations(shapeColumns, shapeRows);

            var canBePlaced = false;

            foreach (var number in squareList)
            {
                var shapeCanBePlaced = true;
                foreach (var comp in originalShapeFilledUpSquares.Select(squareToCheck =>
                             _board.BoardSquares[number[squareToCheck]].GetComponent<BoardSquare>()))
                    if (comp.SquareOccupied)
                        shapeCanBePlaced = false;

                if (shapeCanBePlaced)
                    canBePlaced = true;
            }

            return canBePlaced;
        }

        private List<int[]> GetAllSquaresCombinations(int columns, int rows)
        {
            var squareList = new List<int[]>();
            var lastColumn = 0;
            var lastRow = 0;

            var safeIndex = 0;

            while (lastRow + (rows - 1) < _board.Size)
            {
                var rowData = new List<int>();

                for (var row = lastRow; row < lastRow + rows; row++)
                for (var col = lastColumn; col < lastColumn + columns; col++)
                    rowData.Add(GetComponent<LineIndicator>().LineData[row, col]);

                squareList.Add(rowData.ToArray());

                lastColumn++;

                if (lastColumn + (columns - 1) >= _board.Size)
                {
                    lastRow++;
                    lastColumn = 0;
                }

                safeIndex++;

                if (safeIndex > MaxCombinationCount)
                    break;
            }

            return squareList;
        }
    }
}