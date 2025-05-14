using System;
using System.Collections.Generic;
using Shape;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SquareTextureData", menuName = "ScriptableObjects/SquareTextureData", order = 1)]
    [Serializable]
    public class SquareTextureData : ScriptableObject
    {
        [field: SerializeField] public List<TextureData> ActiveSquareTextures { get; private set; }
        
        [field: SerializeField] public Config.SquareColor StartColor { get; private set; }

        public int GetStartColorIndex()
        {
            return ActiveSquareTextures.FindIndex(x => x.SquareColor == StartColor);
        }

        [Serializable]
        public class TextureData
        {
            [field: SerializeField] public Sprite Texture { get; set; }
            [field: SerializeField] public Config.SquareColor SquareColor { get; set; }
        }
    }
}