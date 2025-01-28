using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private bool chargedBullet;

    [HideInInspector]
    public float bulletDirection = 1;
    private Rigidbody2D rigidBody;
    private float destroyTime = 2;

    LayerMask layerMask = 8;

    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyTime);
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(bulletSpeed * bulletDirection, rigidBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")) return;
        
        if(collider.TryGetComponent(out Enemy enemy)){
            Destroy(enemy.gameObject);
        }

        if(chargedBullet && collider.TryGetComponent(out Breakable breakable)){
            Destroy(breakable.gameObject);
        }

        Destroy(gameObject);
    }
}
