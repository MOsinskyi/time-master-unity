using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class SoundBase
    {
        [field: SerializeField] public Config.AudioName Name { get; set; }
        [field: SerializeField] public AudioClip Clip { get; set; }
        [field: SerializeField] public float Volume { get; set; }
    }
}