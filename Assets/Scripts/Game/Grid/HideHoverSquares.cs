using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class HideHoverSquares : MonoBehaviour
    {
        private ShapePlacement _shapePlacement;
        private Board _board;

        private void OnEnable()
        {
            GameEvents.OnDrag += OnDrag;
        }

        private void OnDrag()
        {
            var hoverSquaresIndexes = new List<int>();
            if (_shapePlacement.CheckIfShapeCanBePlaced(out var squareNumber, out var squareIndexes))
            {
                _board.HoverBoardSquares(squareIndexes);
                hoverSquaresIndexes.AddRange(squareIndexes);
            }
            else
            {
                _board.ClearHoverSquares(hoverSquaresIndexes);
            }
        }

        private void OnDisable()
        {
            GameEvents.OnDrag -= OnDrag;
        }

        private void Start()
        {
            _board = GetComponent<Board>();
            _shapePlacement = GetComponent<ShapePlacement>();
        }
    }
}
