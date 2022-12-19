using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public ObjectPool bulletpool;
    public GameObject bulletPrfab;
    public Transform Muzzle;

    public int currentAmmo;  // ammo
    public int maxAmmo;      
    public bool infiniteAmmo;

    public float bulletSpeed;
    public float shootRate;   
    private float lastShootTime;
    public bool isPlayer;

    public AudioClip shootSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        if (GetComponent<player>()) {
            isPlayer = true;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public bool canShoot()
    {
       
        //the time at the beginning of frame
        if (Time.time - lastShootTime >= shootRate)
        {
         
            if (currentAmmo > 0 || infiniteAmmo == true)
            {
               
                return true;   
                // the return statement simply specifies what the output returned from a function is
            }
        }

        return false;
    }
    public void Shoot()
    {

        lastShootTime = Time.time;
        currentAmmo -= 1;

        if (isPlayer) 
        UI.instance.UpdateAmmoText(currentAmmo, maxAmmo);

    
        audioSource.PlayOneShot(shootSFX);
   
        GameObject bullet = bulletpool.GetObject();
     

        bullet.transform.position = Muzzle.position;
        bullet.transform.rotation = Muzzle.rotation;

        bullet.GetComponent<Rigidbody>().velocity = Muzzle.forward * bulletSpeed;
    }
    // Start is called before the first frame update
    
}
