using System;
using ScriptableObjects;
using UnityEngine;

namespace Game.Shape
{
    public class ShapeColor : MonoBehaviour
    {
        [SerializeField] private SquareTextureData squareTextureData;
        [SerializeField] private int shapeIndex;

        private ShapeRenderer _shapeRenderer;

        private Config.SquareColor _startColor;
        private Config.SquareColor _currentColor;
        private Config.SquareColor _lastColor;
        
        private void Start()
        {
            _shapeRenderer = GetComponent<ShapeRenderer>();
            SetStartColor();
        }

        private void OnEnable()
        {
            GameEvents.UpdateShapeColor += UpdateColor;
        }

        private void OnDisable()
        {
            GameEvents.UpdateShapeColor -= UpdateColor;
        }

        private void SetStartColor()
        {
            _startColor = (Config.SquareColor)shapeIndex + squareTextureData.GetStartColorIndex();
            _currentColor = _startColor;
            _lastColor = _startColor;
            foreach (var square in _shapeRenderer.CurrentShape)
                square.GetComponent<ShapeSquare>().NormalImage.sprite = squareTextureData.ActiveSquareTextures[(int)_startColor].Texture;
        }

        private void UpdateColor()
        {
            if (GameEvents.UpdateShapeColor == null) return;
            _currentColor++;

            if ((int)_currentColor > squareTextureData.ActiveSquareTextures.Count - 1)
                _currentColor = _startColor;
        }

        public void UpdateSprites()
        {
            _lastColor = _currentColor;
            foreach (var square in _shapeRenderer.CurrentShape)
                square.GetComponent<ShapeSquare>().NormalImage.sprite = squareTextureData.ActiveSquareTextures[(int)_currentColor].Texture;
        }

        public Sprite GetCurrentActiveSquareColor()
        {
            return squareTextureData.ActiveSquareTextures[(int)_lastColor].Texture;
        }
    }
}