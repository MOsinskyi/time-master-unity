using System.Collections.Generic;
using System.Linq;
using Shape;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Shape
{
    public class ShapeStorage : MonoBehaviour
    {
        [SerializeField] private List<ShapeData> shapeData;
        [field: SerializeField] public List<ShapeRenderer> ShapeList { get; private set; }

        private void Awake()
        {
            foreach (var shape in ShapeList)
            {
                var shapeIndex = Random.Range(0, shapeData.Count);
                shape.CreateShape(shapeData[shapeIndex]);
            }
        }

        private void OnEnable()
        {
            GameEvents.RequestNewShapes += RequestNewShapes;
        }
        

        private void OnDisable()
        {
            GameEvents.RequestNewShapes -= RequestNewShapes;
        }

        public int ShapeCount()
        {
            return ShapeList.Count;
        }

        public bool IsShapeSquareActive(int index)
        {
            return ShapeList[index].IsAnyOfShapeSquareActive();
        }

        public void ActivateShape(int index)
        {
            ShapeList[index].ActivateShape();
        }

        private void RequestNewShapes()
        {
            foreach (var shape in ShapeList)
            {
                var shapeIndex = Random.Range(0, shapeData.Count);
                shape.RequestNewShape(shapeData[shapeIndex]);
                shape.GetComponent<ShapeColor>().UpdateSprites();
            }
        }

        public int ShapesLeft()
        {
            return ShapeList.Count(shape => shape.IsAnyOfShapeSquareActive() && shape.Drag.IsOnStartPosition());
        }

        public ShapeRenderer GetCurrentSelectedShape()
        {
            foreach (var shape in ShapeList.Where(shape =>
                         !shape.Drag.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive()))
                return shape;

            Debug.LogError("There is no shape selected");
            return null;
        }
    }
}