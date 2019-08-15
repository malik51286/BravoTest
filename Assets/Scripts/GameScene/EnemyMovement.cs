using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    private float horizontal;
    private float vertical;
    private Rigidbody2D rb2d;
    private IMovable movable;
    private int _level;
    private float _minFireDelay;
    private float _maxFireDelay;
    private float _minDirChangeDelay;
    private float _maxDirChangeDelay;

    private Icanon canon;
    private Direction[] direction = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

    void Start()
    {
        SetLevel(1);
        rb2d = GetComponent<Rigidbody2D>();
        movable = GetComponent<IMovable>();
        canon = GetComponent<Icanon>();
        RandomDirection();
        this.PerformActionWithDelay(Random.Range(_minFireDelay, _maxFireDelay), RandomFire);
    }

    public void SetLevel(int level)
    {
        _level = level;
        _minFireDelay = 1f;
        _minDirChangeDelay = 2f;

        _maxFireDelay = 5f - level * 0.1f;
        if (_maxFireDelay < 2f) _maxFireDelay = 2f;

        _maxDirChangeDelay = 7f - level * 0.1f;
        if (_maxDirChangeDelay < 3f) _maxDirChangeDelay = 3f;

    }
    private void RandomFire()
    {
        canon.Fire();
        this.PerformActionWithDelay(Random.Range(_minFireDelay, _maxFireDelay), RandomFire);
    }

    private void RandomDirection()
    {
        Direction selection = direction[Random.Range(0, 4)];
        if (selection == Direction.Up)
        {
            vertical = 1;
            horizontal = 0;
        }
        if (selection == Direction.Down)
        {
            vertical = -1;
            horizontal = 0;
        }
        if (selection == Direction.Right)
        {
            vertical = 0;
            horizontal = 1;
        }
        if (selection == Direction.Left)
        {
            vertical = 0;
            horizontal = -1;
        }
        this.PerformActionWithDelay(Random.Range(_minDirChangeDelay, _maxDirChangeDelay), RandomDirection);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        movable.SetIdle();
        RandomDirection();
    }

    private void FixedUpdate()
    {
        if (vertical != 0 && _isMoving == false)
        {
            StartCoroutine(MoveVertical(vertical, rb2d,movable));
            movable.SetMoving();
        }
        else if (horizontal != 0 && _isMoving == false)
        {
            StartCoroutine(MoveHorizontal(horizontal, rb2d,movable));
            movable.SetMoving();
        }
    }
}