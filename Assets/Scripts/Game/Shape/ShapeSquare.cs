using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Shape
{
    public class ShapeSquare : MonoBehaviour
    {
        [SerializeField] private Image normalImage;
        
        public Image NormalImage => normalImage;

        public void DeactivateShape()
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }

        public void ActivateShape()
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.SetActive(true);
        }
    }
}