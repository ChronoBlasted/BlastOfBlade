using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NONE,
    UP,
    RIGHT,
    DOWN,
    LEFT,
}
public class FanController : MonoBehaviour
{
    [SerializeField] Direction _direction;

    public Direction Direction { get => _direction; }
}
