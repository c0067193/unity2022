using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Health,
    Ammo,
    strengthen
}
public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int value;

    [Header("Moving")]
    public float rotateSpeed;
    public float bobSpeed;
    public float bobHeight;

    private Vector3 startPosition;
    private bool bobbingUp;
    private Rigidbody rig;

    public AudioClip pickupSFX;
    public player player;

    private void Awake()
    {
        rig = player.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //set the start position in our pickup item
        startPosition = transform.position;
    }

    private void Update()
    {
      
        //rotate
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        //bob up and down
        Vector3 offset = (bobbingUp == true ? new Vector3(0, bobHeight / 2, 0) : new Vector3(0, -bobHeight / 2, 0));

        transform.position = Vector3.MoveTowards(transform.position, startPosition + offset, bobSpeed * Time.deltaTime);

        if (transform.position == startPosition + offset)
            bobbingUp = !bobbingUp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            player player = other.GetComponent<player>();
            
            switch (type)
            {
                case PickupType.Health:
                  
                    player.GiveHealth(value);
                    break;
                case PickupType.Ammo:
                    player.GiveMmmo(value);
                    break;
                case PickupType.strengthen:
                    rig.mass = 0.5f;
                    player.jumpspeed = 120f;
                    break;
              }
            other.GetComponent<AudioSource>().PlayOneShot(pickupSFX);
            Destroy(gameObject);
        }

       
    }
}
