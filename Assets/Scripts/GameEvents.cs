using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action GameOver;

    public static Action OnDrag;
    
    public static Action StartReviveCooldown;

    public static Action<int> AddScore;

    public static Action CheckIfShapeCanBePlaced;

    public static Action MoveShapeToStartPosition;

    public static Action RequestNewShapes;

    public static Action SetShapeInactive;

    public static Action UpdateShapeColor;
}