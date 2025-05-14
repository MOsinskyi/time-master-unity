using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class Board : MonoBehaviour
    {
        public int Size => 8;
        public List<GameObject> BoardSquares { get; } = new();
        public Score UIScore { get; private set; }

        private void Awake()
        {
            gameObject.AddComponent<ShapePlacement>();
            gameObject.AddComponent<LineIndicator>();
            gameObject.AddComponent<LineCompletion>();
            gameObject.AddComponent<GameState>();
            gameObject.AddComponent<HideHoverSquares>();

            UIScore = FindAnyObjectByType<Score>();
        }

        public void HoverBoardSquares(List<int> squareIndexes)
        {
            foreach (var squareIndex in squareIndexes)
            {
                BoardSquares[squareIndex].GetComponent<BoardSquare>().EnableHoverImage();
            }
        }

        public void ClearHoverSquares(List<int> hoverSquaresIndexes)
        {
            foreach (var squareIndex in hoverSquaresIndexes)
            {
                BoardSquares[squareIndex].GetComponent<BoardSquare>().DisableHoverImage();
            }
        }
    }
}