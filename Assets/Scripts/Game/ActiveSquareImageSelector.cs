using System.Linq;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ActiveSquareImageSelector : MonoBehaviour
    {
        [SerializeField] private SquareTextureData squareTextureData;
        [SerializeField] private bool updateImageOnReachedTrashHold;

        // private void OnEnable()
        // {
        //     UpdateSquareColorBaseOnCurrentPoints();
        //
        //     if (updateImageOnReachedTrashHold) GameEvents.UpdateSquareColor += UpdateSquaresColor;
        // }

        // private void OnDisable()
        // {
        //     if (updateImageOnReachedTrashHold) GameEvents.UpdateShapeColor -= UpdateSquaresColor;
        // }

        // private void UpdateSquareColorBaseOnCurrentPoints()
        // {
        //     foreach (var squareTexture in squareTextureData.ActiveSquareTextures.Where(squareTexture =>
        //                  squareTextureData.CurrentColor == squareTexture.SquareColor))
        //         GetComponent<Image>().sprite = squareTexture.Texture;
        // }

        private void UpdateSquaresColor(Config.SquareColor color)
        {
            foreach (var squareTexture in squareTextureData.ActiveSquareTextures.Where(squareTexture =>
                         color == squareTexture.SquareColor))
                GetComponent<Image>().sprite = squareTexture.Texture;
        }
    }
}