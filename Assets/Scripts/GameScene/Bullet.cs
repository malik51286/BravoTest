using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour,IDestroyer
{
    private float _speed;
    private float _damage;
    private Tilemap tilemap;
    private Rigidbody2D rb2d;

    public float speed
    {
        get => _speed;
        set => _speed = value;
    }
    public float damage
    {
        get => _damage;
        set => _damage = value;
    }

    void Awake()
    {
        _damage = 1;
        _speed = 10;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rb2d.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb2d.velocity = Vector2.zero;
        if (collision.gameObject.GetComponent<IDestroyable>() != null)
        {
            collision.gameObject.GetComponent<IDestroyable>().InflictDamage(_damage);
        }
        
        else if(collision.gameObject.GetComponent<Tilemap>() != null)
        {
            tilemap = collision.gameObject.GetComponent<Tilemap>();
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
               hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
               hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
               tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
               break;
            }
        }

        Destroy();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        
        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject(ResourceList.Explosion);
        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
            explosion.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            explosion.SetActive(true);
        }
        
    }

}
