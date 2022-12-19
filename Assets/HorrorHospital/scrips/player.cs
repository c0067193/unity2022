using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [Header("stats")]
    public int currentHP;
    public int maxHP;

    [Header("Movement")]
    public float moveSpeed ;       //movement speed in units per second
    public int jumpForce ;       //force apply upwards
    public float jumpspeed;
    public float gravity = 20;
    public bool isjump = false;
    public float jumptime = 0.5f;
    public float jumptimeflag;
    
    
    [Header("Camera")]
    public float lookSensitivity ;  //mouse look sensitivity
    public float maxLookX ;         //highest x rotation of the camera
    public float minLookX ;         //lowest down we can look
    private float rotx;           //current x ratation of the camera
    public float pickupDistance;//pickup distance
    public LayerMask pickupmask;
    
    private Camera cam;
    private Rigidbody rig;
    private weapon weapon;
    public CharacterController controller;
    public int score = 0;

    private void Awake()
    {
        //get the components
        cam = Camera.main;
        rig = GetComponent<Rigidbody>();
        weapon = GetComponent<weapon>();

        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //initialize the UI
        UI.instance.updatehealthBar(currentHP, maxHP);
        UI.instance.UpdateScoreText(0); 
        UI.instance.UpdateAmmoText(weapon.currentAmmo, weapon.maxAmmo);
       
    }

    // Update is called once per frame
     private void Update()
    {
        //dont do anything if the game is paused
        if (GameManager.instance.gamePaused == true)
            return;
        
        Move();
       
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("bbb");
            isjump = true;
           
            //called one time
            TryJump();
        }
        
        if (Input.GetButton("Fire1")) //called everytime
        {
            
            if (weapon.canShoot())
            {
               
                weapon.Shoot();
            }
            

        }

        if (Cursor.lockState == CursorLockMode.Locked)
            CameraLOOK();
         
        Pickupguns();


    }

    private void Move()
    {
   
         float x = Input.GetAxis("Horizontal") * moveSpeed;
         float z = Input.GetAxis("Vertical") * moveSpeed;

        //  rig.velocity = new Vector3(x, rig.velocity.y, z);
        Vector3 dir = transform.right * x + transform.forward * z;
         dir.Normalize();
         dir *= moveSpeed * Time.deltaTime;
        controller.SimpleMove(dir);
       // dir.y = rig.velocity.y;
        rig.velocity = dir;
    }
    private void CameraLOOK()
    {
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotx += Input.GetAxis("Mouse Y") * lookSensitivity;
        rotx = Mathf.Clamp(rotx, minLookX, maxLookX);

        //It rotates around the X-axis
        cam.transform.localRotation = Quaternion.Euler(-rotx, 0, 0);
        //adding the rotation along the y axis
        transform.eulerAngles += Vector3.up * y;
    }
    public void TryJump()
    {
        if (isjump) 
        {
       
            if (jumptimeflag < jumptime)
            {
           
                controller.Move(transform.up * jumpspeed * Time.deltaTime*1/rig.mass);
                jumptimeflag += Time.deltaTime;
            }
            else if (jumptime < jumptimeflag)
            {
                controller.Move(transform.up * -gravity * Time.deltaTime);

            }
            if (controller.collisionFlags == CollisionFlags.Below)
            {
                jumptimeflag = 0;
                isjump = false;
            }
        }
        else
        {
            if (controller.collisionFlags != CollisionFlags.Below)
                controller.Move(transform.up * -gravity * Time.deltaTime);
        }

    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UI.instance.updatehealthBar(currentHP, maxHP);

        if (currentHP <= 0)
            Die();

    }
    void Die()
    {
        GameManager.instance.LoseGame();
    }
    public void GiveHealth(int value)
    {
       
        currentHP  = Mathf.Clamp(currentHP + value, 0 , maxHP);
        UI.instance.updatehealthBar(currentHP, maxHP);
    }
    public void GiveMmmo(int value)
    {

        weapon.currentAmmo  = Mathf.Clamp(weapon.currentAmmo+value,0,weapon.maxAmmo);
        UI.instance.UpdateAmmoText(weapon.currentAmmo, weapon.maxAmmo);
    }
    private void Pickupguns()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("guns");
            }
        }
    }
    
}
