using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject chargedBulletPrefab;

    [SerializeField]
    private Transform bulletSpawnTransform;

    [SerializeField]
    private float chargedShotTime;

    private float spawnPositionXOffset;
    
    private void Start(){
        spawnPositionXOffset = bulletSpawnTransform.localPosition.x;
    }

    public void ShootBullet(int facingDirection, float holdingShotTime){
        Vector3 spawnPosition = bulletSpawnTransform.position;
        spawnPosition.x = transform.position.x + spawnPositionXOffset * facingDirection;
        GameObject spawnedBullet = null;
        if(holdingShotTime < chargedShotTime){
            spawnedBullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        } else{
            spawnedBullet = Instantiate(chargedBulletPrefab, spawnPosition, transform.rotation);
        }
        if(spawnedBullet.TryGetComponent(out Bullet bullet)){
            bullet.bulletDirection = facingDirection;
        }
    }
}
