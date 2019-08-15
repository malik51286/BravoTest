using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : MonoBehaviour,Icanon,IDestroyable,IMovable
{
    #region variables

    private float _health;
    private float _speed;
    private string _bulletType;
    private bool _isMoving;
    private Direction _direction;
    private MovableType _type;
    private Action _destroyAction=null;
    private Animator _animator;

    #endregion

    #region Properties
    public float health
    {
        get => _health;
        set => _health = value;
    }

    public float speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    public Direction direction
    {
        get => _direction;
        set => _direction = value;
    }
    
    public MovableType type
    {
        get { return _type; }
        set
        {
            _type = value;
            switch (_type)
            {
                case MovableType.Player:
                    _bulletType = ResourceList.PlayerBullet;
                    break;

                case MovableType.Enemy:
                    _bulletType = ResourceList.EnemyBullet;
                    break;

                default:
                    _bulletType = ResourceList.EnemyBullet;
                    break;
            }
        }
    }

    public bool isMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }

    #endregion

    void Start()
    {
        _isMoving = false;
        _speed = 5;
        _health = 1;
        _animator = GetComponent<Animator>();
    }

    public void SetDestroyAction(Action action)
    {
        _destroyAction = action;
    }

    public void Fire()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(_bulletType);
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }   
    }
    
    public void SetIdle()
    {
        if (_isMoving)
        {
            if (_animator != null)
            {
                _animator.SetFloat("Movement", 0);
            }
            _isMoving = false;
        }
    }

    public void SetMoving()
    {
        if (!_isMoving)
        {
            if (_animator != null)
            {
                _animator.SetFloat("Movement", 1);
            }
            _isMoving = true;
        }
    }

    public void SetExploding()
    { 
        _animator.SetTrigger("Explode");
    }

    public void InflictDamage(float amount)
    {
        if (_health > 0)
        {
            _health -= amount;
            if (_health <= 0)
            {
                if (_destroyAction != null)
                {
                    _destroyAction();
                }
                Destroy(gameObject.GetComponent<Movement>());
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                Destroy(gameObject.GetComponent<Rigidbody2D>());
                SetExploding();
            }
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
