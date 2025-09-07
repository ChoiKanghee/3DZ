using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    private const int LeftMouseButton = 0;
    public GameObject bulletPrefab;
    public Transform firingPos;
    public float bulletSpeed;
    public AudioSource shootingSound;
    public Animator anim;

    void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            ShootBullet();
        }
    }

    private void ShootBullet() => anim.SetTrigger("Shoot");

    public void PlayFireSound() => shootingSound.Play();

    public void AddProjectile()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPos.position, firingPos.rotation);
        bullet.GetComponent<Rigidbody>().velocity = firingPos.forward * bulletSpeed;
    }
}
