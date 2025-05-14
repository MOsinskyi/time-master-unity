using System.Collections.Generic;
using System.Linq;
using Shape;
using UnityEngine;

namespace Game.Shape
{
    public class ShapeRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject squareShapeImage;
        public List<GameObject> CurrentShape { get; } = new();

        private bool _shapeActive;

        public ShapeData CurrenShapeData { get; private set; }
        public ShapeDrag Drag { get; private set; }
        public int TotalSquareNumber { get; private set; }

        private void Awake()
        {
            Drag = GetComponent<ShapeDrag>();
            _shapeActive = true;
        }

        private void OnEnable()
        {
            GameEvents.SetShapeInactive += SetShapeInactive;
        }

        private void OnDisable()
        {
            GameEvents.SetShapeInactive -= SetShapeInactive;
        }

        public void RequestNewShape(ShapeData shapeData)
        {
            Drag.MoveShapeToStartPosition();
            CreateShape(shapeData);
        }

        public bool IsAnyOfShapeSquareActive()
        {
            return CurrentShape.Any(square => square.gameObject.activeSelf);
        }

        private void SetShapeInactive()
        {
            if (Drag.IsOnStartPosition() || !IsAnyOfShapeSquareActive()) return;
            foreach (var square in CurrentShape)
                square.gameObject.SetActive(false);
        }

        public void ActivateShape()
        {
            if (_shapeActive) return;
            foreach (var square in CurrentShape) square?.GetComponent<ShapeSquare>().ActivateShape();

            _shapeActive = true;
        }

        public void CreateShape(ShapeData shapeData)
        {
            TotalSquareNumber = GetNumberOfSquares(shapeData);
            CurrenShapeData = shapeData;

            while (CurrentShape.Count <= TotalSquareNumber)
                CurrentShape.Add(Instantiate(squareShapeImage, transform));

            foreach (var square in CurrentShape)
            {
                square.transform.position = Vector2.zero;
                square.SetActive(false);
            }

            var squareRect = squareShapeImage.GetComponent<RectTransform>();
            var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
                squareRect.rect.height * squareRect.localScale.y);
            var currentIndexInList = 0;

            for (var row = 0; row < shapeData.Rows; row++)
            for (var column = 0; column < shapeData.Columns; column++)
                if (shapeData.Board[row].Column[column])
                {
                    CurrentShape[currentIndexInList].SetActive(true);
                    CurrentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = new Vector2(
                        GetXPositionForShapeSquare(shapeData, column, moveDistance),
                        GetYPositionForShapeSquare(shapeData, row, moveDistance));

                    currentIndexInList++;
                }
        }

        private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
        {
            var shiftOnY = 0f;

            if (shapeData.Rows <= 1) return shiftOnY;
            if (shapeData.Rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.Rows - 1) / 2;
                var multiplier = (shapeData.Rows - 1) / 2;

                if (row < middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = shapeData.Rows == 2 ? 1 : shapeData.Rows / 2;
                var middleSquareIndex1 = shapeData.Rows == 2 ? 0 : shapeData.Rows - 2;

                var multiplier = shapeData.Rows / 2;

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                        shiftOnY = moveDistance.x / 2 * -1;
                    if (row == middleSquareIndex1)
                        shiftOnY = moveDistance.x / 2;
                }

                if (row < middleSquareIndex2 && row < middleSquareIndex1)
                {
                    shiftOnY = moveDistance.x * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex2 && row > middleSquareIndex1)
                {
                    shiftOnY = moveDistance.x * -1;
                    shiftOnY *= multiplier;
                }
            }

            return shiftOnY;
        }

        private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
        {
            var shiftOnX = 0f;

            if (shapeData.Columns <= 1) return shiftOnX;
            if (shapeData.Columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.Columns - 1) / 2;
                var multiplier = (shapeData.Columns - 1) / 2;

                if (column < middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = shapeData.Columns == 2 ? 1 : shapeData.Columns / 2;
                var middleSquareIndex1 = shapeData.Columns == 2 ? 0 : shapeData.Columns - 1;

                var multiplier = shapeData.Columns / 2;

                if (column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2)
                        shiftOnX = moveDistance.x / 2;
                    if (column == middleSquareIndex1)
                        shiftOnX = moveDistance.x / 2 * -1;
                }

                if (column < middleSquareIndex2 && column < middleSquareIndex1)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex2 && column > middleSquareIndex1)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }

            return shiftOnX;
        }

        private int GetNumberOfSquares(ShapeData shapeData)
        {
            return shapeData.Board.SelectMany(rowData => rowData.Column).Count(active => active);
        }
    }
}