using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
   
    public int damage;
    public float lifeTime;
    private float shootTime;
    public GameObject HitEffect;
    public int Damage;

    private void OnEnable()
    {
        //shootTime = Time.time;
    }

    private void Update()
    {

        //disable the bullet after lifetime
       // if (Time.time - shootTime >= lifeTime)
          //  gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {

        //did we hit the player
        if (other.CompareTag("Player"))
        {

           
        }
        
        else if (other.CompareTag("Enemy2"))
        {
            Debug.Log("no");
            other.GetComponent<Enemy2>().TakeDamage(damage);

            //create the hit effect
            GameObject obj = Instantiate(HitEffect, transform.position, Quaternion.identity);
            Destroy(obj, 0.5f);
        }
        else if (other.CompareTag("Head"))
            {
            Debug.Log("yes");
            other.GetComponent<Headcollider>().TakeHeadDamage(damage);

            //create the hit effect
            GameObject obj = Instantiate(HitEffect, transform.position, Quaternion.identity);
            Destroy(obj, 0.5f);
        }
        //else if(other.CompareTag("Ground"))
        //disable the bullet after hit the target
       // gameObject.SetActive(false);
    }
    //private void OnCollisionEnter(Collision collision)
    // {
    //
    //did we hit the player
    // if (collision.transform.tag == "Player")
    // {

    //   collision.transform.GetComponent<player>().TakeDamage(damage);
    // }
    // else if (collision.transform.tag == "Enemy")
    // {
    //      Debug.Log("qwe"); collision.transform.GetComponent<Enemy>().TakeDamage(damage);
    //  }
    // //disable the bullet after hit the target
    //  gameObject.SetActive(false);
    // }


}
