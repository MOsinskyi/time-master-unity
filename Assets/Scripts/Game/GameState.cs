using Game.Grid;
using Game.Shape;
using Shape;
using UnityEngine;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private ShapePlacement _shapePlacement;
        private ShapeStorage _shapeStorage;

        private void Start()
        {
            _shapeStorage = FindAnyObjectByType<ShapeStorage>();
            _shapePlacement = FindAnyObjectByType<ShapePlacement>();
        }

        public void CheckIfPlayerLost()
        {
            var validShapes = 0;

            for (var i = 0; i < _shapeStorage.ShapeCount(); i++)
            {
                var isShapeActive = _shapeStorage.IsShapeSquareActive(i);
                if (!_shapePlacement.CheckIfShapeCanBePlacedOnGrid(_shapeStorage.ShapeList[i]) ||
                    !isShapeActive) continue;
                _shapeStorage.ActivateShape(i);
                validShapes++;
            }

            if (validShapes == 0) GameEvents.StartReviveCooldown?.Invoke();
        }
    }
}