using UnityEngine;

namespace Game.Grid
{
    public class GridCreator : MonoBehaviour
    {
        private const float SquareGap = 0;

        [SerializeField] private GameObject boardSquarePrefab;
        [SerializeField] private Vector2 startPosition = new(-363f, 500f);
        [Range(1f, 1.1f)] [SerializeField] private float squareScale = 1.05f;
        private Board _board;

        private float _everySquareOffset;

        private Vector2 _offset = Vector2.zero;

        public float SquareScale => squareScale;

        private void Awake()
        {
            _board = GetComponent<Board>();
            CreateGrid();
        }

        private void CreateGrid()
        {
            SpawnGridSquares();
            SetGridSquaresPosition();
        }

        private void SetGridSquaresPosition()
        {
            var columnNumber = 0;
            var rowNumber = 0;
            var squareGapNumber = Vector2.zero;
            var rowMoved = false;

            var squareRect = _board.BoardSquares[0].GetComponent<RectTransform>();

            _offset.x = squareRect.rect.width * squareRect.transform.localScale.x + _everySquareOffset;
            _offset.y = squareRect.rect.height * squareRect.transform.localScale.y + _everySquareOffset;

            foreach (var square in _board.BoardSquares)
            {
                if (columnNumber + 1 > _board.Size)
                {
                    squareGapNumber.x = 0;
                    columnNumber = 0;
                    rowNumber++;
                    rowMoved = false;
                }

                var posXOffset = _offset.x * columnNumber + squareGapNumber.x * SquareGap;
                var posYOffset = _offset.y * rowNumber + squareGapNumber.y * SquareGap;

                if (columnNumber > 0 && columnNumber % 3 == 0)
                {
                    squareGapNumber.x++;
                    posXOffset += SquareGap;
                }

                if (rowNumber > 0 && rowNumber % 3 == 0 && !rowMoved)
                {
                    rowMoved = true;
                    squareGapNumber.y++;
                    posYOffset += SquareGap;
                }

                square.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(startPosition.x + posXOffset, startPosition.y - posYOffset);
                square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + posXOffset,
                    startPosition.y - posYOffset, 0);

                columnNumber++;
            }
        }

        private void SpawnGridSquares()
        {
            var squareIndex = 0;

            for (var row = 0; row < _board.Size; row++)
            for (var col = 0; col < _board.Size; col++)
            {
                _board.BoardSquares.Add(Instantiate(boardSquarePrefab, transform));
                _board.BoardSquares[^1].GetComponent<BoardSquare>().SquareIndex = squareIndex;
                _board.BoardSquares[^1].transform.localScale = new Vector3(squareScale, squareScale, 1);

                squareIndex++;
            }
        }
    }
}