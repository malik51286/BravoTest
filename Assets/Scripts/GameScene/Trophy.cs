using UnityEngine;
using System.Collections;
using System;

public class Trophy : MonoBehaviour,IDestroyable
{
    private float _health;
    private Action _destroyAction=null;
    public float health
    {
        get => _health;
        set => _health = value;
    }

    void Start()
    {
        _health = 1;
    }

    public void SetDestroyAction(Action action)
    {
        _destroyAction = action;
    }
   
    public void SetExploding()
    {
        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject(ResourceList.Explosion);
        if (explosion != null)
        {
            explosion.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
            explosion.SetActive(true);
        }
    }
    public void InflictDamage(float amount)
    {
        if (_health > 0)
        {
            _health -= amount;
            if (_health <= 0)
            {
                Destroy();
            }
        }
    }
    public void Destroy()
    {
        if (_destroyAction != null)
        {
            _destroyAction();
        }
        SetExploding();
        Destroy(this.gameObject);
    }
}
