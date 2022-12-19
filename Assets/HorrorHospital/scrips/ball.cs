using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public int DropTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Drop",DropTime );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Drop()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
