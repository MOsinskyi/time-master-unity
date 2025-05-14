using UnityEngine;
using UnityEngine.UI;

namespace Game.Grid
{
    public class BoardSquare : MonoBehaviour
    {
        [SerializeField] private Image normalImage;
        [SerializeField] private Image hooverImage;
        [SerializeField] private Image activeImage;
        
        public bool Selected { get; internal set; }
        public int SquareIndex { get; internal set; }
        public bool SquareOccupied { get; private set; }

        private void Start()
        {
            ClearOccupied();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!SquareOccupied)
                Selected = true;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (SquareOccupied) return;
            Selected = false;
            DisableHoverImage();

        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            Selected = true;
        }

        public void EnableHoverImage()
        {
            hooverImage.gameObject.SetActive(true);
        }

        public void DisableHoverImage()
        {
            hooverImage.gameObject.SetActive(false);
        }

        public void Deactivate()
        {
            activeImage.gameObject.SetActive(false);
        }

        public void ClearOccupied()
        {
            Selected = false;
            SquareOccupied = false;
        }

        public void ActivateSquare()
        {
            hooverImage.gameObject.SetActive(false);
            activeImage.gameObject.SetActive(true);
            Selected = true;
            SquareOccupied = true;
        }

        public void PlaceShapeOnBoard(Sprite newSquareTexture)
        {
            activeImage.sprite = newSquareTexture;
            ActivateSquare();
        }
    }
}