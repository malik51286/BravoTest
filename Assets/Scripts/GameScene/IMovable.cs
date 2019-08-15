using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public enum MovableType
{
    Player,
    Enemy
}
public interface IMovable
{
    float speed { get; set; }
    bool isMoving { get; set; }
    Direction direction { get; set; }
    MovableType type { get; set; }
    void Fire();
    void SetIdle();
    void SetMoving();
}
